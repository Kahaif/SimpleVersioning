﻿using Microsoft.EntityFrameworkCore;
using SimpleVersioning.Data;
using SimpleVersioning.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace SimpleVersioning.Extensions
{
    public static class EFCoreExtensions
    {
        /// <summary>
        /// Add the AND clauses to filter through the names and min/max versions.
        /// </summary>
        /// <param name="files">The files.</param>
        /// <param name="name">The name.</param>
        /// <param name="minVersion">The minimum version.</param>
        /// <param name="maxVersion">The maximum version.</param>
        public static IQueryable<File> BuildQueryWithComparison(this DbSet<File> files, string name, string minVersion, string maxVersion)
        {
            
            string[] parameters = { "", "", "" };
            string sqlConditions = "";
           
            if (name != "")
            {
                sqlConditions = " AND Files.ResourceName = {0}";
                parameters[0] = name;
            }

            if (minVersion != "")
            {
                sqlConditions += " AND FileVersions.Version >= {1}";
                parameters[1] = minVersion;
            }

            if (maxVersion != "")
            { 
                sqlConditions += " AND FileVersions.Version <= {2}";
                parameters[2] = maxVersion;
            }

            if (sqlConditions != "")
                
                return files.FromSqlRaw("SELECT Files.* FROM Files, FileVersions WHERE Files.Id = FileVersions.FileId" + sqlConditions, parameters);
            else
                return files;

        }

        /// <summary>
        /// Compare the list of tuples with the properties of the file version
        /// </summary>
        /// <param name="files">The files.</param>
        /// <param name="propertyAndConditions">The property and conditions.</param>
        /// <exception cref="ArgumentException">propertyAndConditions - Invalid comparator for {propertyAndCondition.Item1} with value {propertyAndCondition.Item3}</exception>
        public static IQueryable<File> CompareFileProperties(this DbSet<File> files, List<Tuple<string, char, string>> propertyAndConditions)
        {
            foreach (var propertyAndCondition in propertyAndConditions)
            {
                char comparator = propertyAndCondition.Item2;
                // Check if the comparator is a valid one (as in, in the comparator enum)
                if (!Enum.IsDefined(typeof(Comparators), comparator))
                    throw new ArgumentException(nameof(propertyAndConditions), $"Invalid comparator for {propertyAndCondition.Item1} with value {propertyAndCondition.Item3}");


                var command = $"SELECT * FROM dbo.Files WHERE Id = (SELECT FileId FROM dbo.FileProperties WHERE Name = {propertyAndCondition.Item1} AND Value ";

                if (comparator != (char)Comparators.Different)
                    command += $"{comparator}";
                else
                    command += "<>";

                command += $" {comparator}";

                files.FromSqlInterpolated(FormattableStringFactory.Create(command));
            }
            return files;
        }

        public static IQueryable<File> Sort(this IQueryable<File> query, FileSort sort)
        {
            
            if ((sort & FileSort.Name) == FileSort.Name) query = query.OrderBy(x => x.Name);
            if ((sort & FileSort.Version) == FileSort.Version) query = query.OrderBy(x => x.Versions.Select(v => v.Version));
            if ((sort & FileSort.LastUpdatedTime) == FileSort.LastUpdatedTime) query = query.OrderBy(x => x.Versions.Select(v => v.LastUpdatedTime));
            if ((sort & FileSort.CreationTime) == FileSort.CreationTime) query = query.OrderBy(x => x.Versions.Select(v => v.CreationTime));
            return query;
        }
    }
}
