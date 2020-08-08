using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SimpleVersioning.Data;
using SimpleVersioning.Extensions;
using SimpleVersioning.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SimpleVersioning.Controllers
{
    [Route("/files")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        IStorageRepositoryAsync storageRepository;
        ILogger logger;

        public FilesController(IStorageRepositoryAsync storageRepository, ILoggerFactory loggerFactory, IWebHostEnvironment env)
        {
            this.storageRepository = storageRepository;
            logger = loggerFactory.CreateLogger<FilesController>();
        }

        private List<string> CheckVersionsForErrors(List<FileVersion> fileVersions)
        {
            var errors = new Lazy<List<string>>();
            if (fileVersions.Count == 0)
                errors.Value.Add("No version to add");

            for (int i = 0; i < fileVersions.Count; i++)
            {
                string pos = $"Version at position {i} : ";
                if (string.IsNullOrWhiteSpace(fileVersions[i].Path) && fileVersions[i].Content.Length == 0)
                    errors.Value.Add($"{pos} No path or content");

                if (string.IsNullOrWhiteSpace(fileVersions[i].Type))
                    errors.Value.Add($"{pos} Must have a type");

            }

            if (errors.IsValueCreated)
                return errors.Value;

            return null;
        }
        /// <summary>
        /// Parse files to check for errors of format
        /// </summary>
        /// <param name="files">Files to parse</param>
        /// <returns>A string containing the errors</returns>
        private List<string> CheckFilesForErrors(List<File> files)
        {
            var errors = new Lazy<List<string>>();
            if (files.Count == 0)
            {
                errors.Value.Add("No items");
                return errors.Value;
            }

            // No special chars, hyphens and alphanumeric only 
            var ressourceRegex = new Regex("^[_A-z0-9]*((-|)*[_A-z0-9])*$", RegexOptions.IgnoreCase);

            for (int i = 0; i < files.Count; i++)
            {
                string pos = $"File at position {i} : ";
                File file = files[i];

                if (string.IsNullOrWhiteSpace(file.Name)) 
                    errors.Value.Add(pos + "Name can't be empty");

                if (!ressourceRegex.IsMatch(file.ResourceName)) 
                    errors.Value.Add(pos + "RessourceName is in an invalid form");

                if (file.Versions == null || file.Versions.Count == 0) 
                    errors.Value.Add(pos + "Has no versions");

                var versionsErrors = CheckVersionsForErrors(files[i].Versions);
                if (versionsErrors != null) errors.Value.AddRange(versionsErrors);
            }

            if (errors.IsValueCreated) return errors.Value;

            return null;
        }

        [HttpGet("/files/{resourceName}")]
        public ActionResult<IAsyncEnumerable<File>> GetByResourceName(string resourceName, [FromQuery] string sort, [FromQuery] string from, [FromQuery] string to, [FromQuery] string minVersion, [FromQuery] string maxVersion)
        {
            try
            {
                FileSort fileSort = sort switch
                {
                    "version" => FileSort.Version,
                    "lastUpdatedTime" => FileSort.LastUpdatedTime,
                    "creationTime" => FileSort.CreationTime,
                    "name" => FileSort.Name,
                    _ => FileSort.Name
                };

                DateTime toDateTime = DateTime.ParseExact(to, "dd-mm-yyyy", null),
                         fromDateTime = DateTime.ParseExact(from, "dd-mm-yyyy", null);
                //storageRepository.GetFilesAsync(fromDateTime, toDateTime, resourceName, minVersion, maxVersion, fileSort)

                return Ok();
            }
            catch (Exception e)
            {
                logger.LogError($"Error in POST/files : Exception thrown with query  {Request.QueryString} and error {e.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet]
        public Task<ActionResult<List<File>>> Get([FromQuery] string sort, [FromQuery] string from, [FromQuery] string to, [FromQuery] string minVersion, [FromQuery] string maxVersion)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Add a file in the storage repository
        /// </summary>
        /// <returns>The created item and the "Location" HTTP header which shows how to access it</returns>
        /// <remarks>
        /// Sample :
        /// POST /files
        /// {
        ///     "name": "MyApp",
        ///     
        /// }
        /// </remarks>
        /// <response code="201">Return the created files</response>
        /// <response code="400">If the files are null or if they miss sone fields</response>
        /// <response code="500">If the files couldn't be added in the storage repo</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<File>> Add([FromBody] File file)
        {
            var body = await Request.Body.GetStringFromStreamAsync(Encoding.UTF8);
            if (file == null)
            {
                logger.LogError("POST/files: couldn't parse body " + body);
                return BadRequest(new { error = "Couldn't parse the body to get the files to save" });
            }

            List<string> errors = CheckFilesForErrors(new List<File>() { file });
            if (errors != null)
                return BadRequest(errors);

            try
            {
                // Every item must be correctly added in the repo
                if (await storageRepository.AddAsync(file)) 
                {
                    return Created($"/files/{file.ResourceName}", file);
                }
                else
                {
                    logger.LogError($"Error in POST/files : files couldn't be added with body {body} and items {Environment.NewLine}{JsonConvert.SerializeObject(file)}");
                    return StatusCode(StatusCodes.Status500InternalServerError);
                }
            }
            catch(Exception e)
            {
                logger.LogError($"Error in POST/files : Exception thrown with body  {body} and error {e.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Adds the versions to the file that has the given ResourceName.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="fileVersions">The file versions.</param>
        /// <returns></returns>
        [HttpPost("/files/{resourceName}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<FileVersion>>> AddVersions(string resourceName, [FromBody] List<FileVersion> fileVersions)
        {
            if (fileVersions == null)
                return BadRequest(new { error = "Couldn't parse the versions to add" });

            var body = await Request.Body.GetStringFromStreamAsync(Encoding.UTF8);

            var errors = CheckVersionsForErrors(fileVersions);
            if (errors != null)
            {
                logger.LogError($"POST/name Couldn't parse body {body}");
                return BadRequest(errors);
            }

            File f = null;
            await foreach (var item in storageRepository.GetFilesAsync(resourceName))
            {
                // Return an error if there's more than one file with the same name
                if (f != null)
                {
                    logger.LogCritical($"Two files with the same resource name exist : {resourceName}");
                    return StatusCode(StatusCodes.Status500InternalServerError);
                }
                f = item;
            }

            try
            {
                f.Versions.AddRange(fileVersions);

                if (await storageRepository.UpdateAsync(f.Id, f))
                {
                    return Created($"/files/{f.ResourceName}/[versions]", f);
                }
                else
                {
                    logger.LogError($"Couldn't add the versions to file {f.Name} with versions {fileVersions}");
                    return StatusCode(StatusCodes.Status500InternalServerError);
                }
            }
            catch (Exception e)
            {
                logger.LogError($"Error in POST/files : Exception thrown with body  {body} and error {e.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

        }

        [HttpPut("/files/{resourceName}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<File>> UpdateByName(string resourceName, [FromBody] File file)
        {
            if (file == null)
                return BadRequest(new { error = "Couldn't parse the file to update" });

            var body = await Request.Body.GetStringFromStreamAsync(Encoding.UTF8);

            try
            {
                int id = 0;
                await foreach (var item in storageRepository.GetFilesAsync(resourceName))
                {
                    // Return an error if there's more than one file with the same name
                    if (id != 0)
                    {
                        logger.LogCritical($"Two files with the same resource name exist : {resourceName}");
                        return StatusCode(StatusCodes.Status500InternalServerError);
                    }
                    id = item.Id;
                }
                
                if (await storageRepository.UpdateAsync(id, file))
                {
                    return Ok(file);
                }
                else
                {
                    logger.LogError($"Error in POST/files : couldn't update file  {body}");
                    return StatusCode(StatusCodes.Status500InternalServerError);
                }
            }
            catch (Exception e)
            {
                logger.LogError($"Error in POST/files : Exception thrown with body  {body} and error {e.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            
        }

        [HttpDelete("/files/{resourceName}")]
        public Task<ActionResult<bool>> Delete(string resourceName)
        {
            throw new NotImplementedException();
        }


        [HttpPut("/files/{resourceName}/{version}")]
        public Task<ActionResult<FileVersion>> UpdateVersion(string resourceName, string version, [FromBody] FileVersion newVersion)
        {
            throw new NotImplementedException();
        }

        [HttpGet("/files/{resourceName}/latest")]
        public Task<ActionResult<File>> GetLatest(string resourceName)
        {
            throw new NotImplementedException();
        }

        [HttpDelete("/files/{resourceName}/{version}")]
        public Task<ActionResult<bool>> DeleteVersion(string resourceName, string version)
        {
            throw new NotImplementedException();
        }

        [HttpGet("/files/{resourceName}/{version}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<File>> GetByNameVersion(string resourceName, string version)
        {
            try
            {
                File f = null;
                await foreach (var item in storageRepository.GetFilesAsync(resourceName, version, version))
                {
                    // Item already exists
                    if (f != null)
                    {
                        logger.LogCritical($"FileVersion with name {resourceName} and version {version} exists twice in the storage repository");
                        return StatusCode(StatusCodes.Status500InternalServerError);
                    }

                    f = item;
                }
                // No item found
                if (f == null)
                {
                    return NotFound();
                }

                return new OkObjectResult(f);
            }
            catch (Exception e)
            {
                logger.LogError($"Error in POST/files : Exception thrown with query  {Request.QueryString} and error {e.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

    }
}
