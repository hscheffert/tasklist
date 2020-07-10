using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using TaskList.Core.DTOs;
using TaskList.Data.Model;

namespace TaskList.Business.Helpers
{
    public static class Areas
    {
        /// <summary>
        /// Gets an Area record by ID
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns>
        /// Area record
        /// </returns>
        public static AreaDTO GetByID(Guid id)
        {
            using (var db = new DB())
            {
                var areaDTO = (from a in db.Area
                               where a.AreaId == id
                               select new AreaDTO()
                               {
                                   AreaId = a.AreaId,
                                   Name = a.Name,
                                   DisplayOrder = a.DisplayOrder,
                                   IsActive = a.IsActive,
                                   CreatedBy = a.CreatedBy,
                                   CreatedDate = a.CreatedDate,
                                   UpdatedBy = a.UpdatedBy,
                                   UpdatedDate = a.UpdatedDate,
                                   SubAreas = a.SubArea
                                        .OrderBy(s => s.DisplayOrder)
                                        .Select(x => new SubAreaDTO()
                                        {
                                            SubAreaId = x.SubAreaId,
                                            AreaId = x.AreaId,
                                            Name = x.Name,
                                            DisplayOrder = x.DisplayOrder,
                                            IsActive = x.IsActive,
                                            CreatedBy = x.CreatedBy,
                                            CreatedDate = x.CreatedDate,
                                            UpdatedBy = x.UpdatedBy,
                                            UpdatedDate = x.UpdatedDate,
                                        })
                               }).FirstOrDefault();

                return areaDTO;
            }
        }

        /// <summary>
        /// Gets All Area records
        /// </summary>
        /// <returns>
        /// List Area
        /// </returns>
        public static List<AreaDTO> GetAll(bool activeOnly = false)
        {
            using (var db = new DB())
            {
                IQueryable<Area> query = db.Area;

                if(activeOnly)
                {
                    query = query.Where(a => a.IsActive);
                }

                var areaDTOs = query
                    .Select(a => new AreaDTO()
                    {
                        AreaId = a.AreaId,
                        Name = a.Name,
                        DisplayOrder = a.DisplayOrder,
                        IsActive = a.IsActive,
                        CreatedBy = a.CreatedBy,
                        CreatedDate = a.CreatedDate,
                        UpdatedBy = a.UpdatedBy,
                        UpdatedDate = a.UpdatedDate,
                        SubAreas = activeOnly ? a.SubArea
                        .Where(s => s.IsActive)
                                        .OrderBy(s => s.DisplayOrder)
                                        .Select(s => new SubAreaDTO()
                                        {
                                            SubAreaId = s.SubAreaId,
                                            AreaId = s.AreaId,
                                            Name = s.Name,
                                            IsActive = s.IsActive,
                                        }) : a.SubArea
                                        .OrderBy(s => s.DisplayOrder)
                                        .Select(s => new SubAreaDTO()
                                        {
                                            SubAreaId = s.SubAreaId,
                                            AreaId = s.AreaId,
                                            Name = s.Name,
                                            IsActive = s.IsActive,
                                        })
                    });

                return areaDTOs
                        .OrderBy(x => x.DisplayOrder)
                        .ToList();
            }
        }

        /// <summary>
        /// Saves an Area record
        /// </summary>
        /// <param name="toSave">Area</param>
        /// <param name="errorMessage"></param>
        /// <returns>
        /// Did the record save properly
        /// </returns>
        public static Guid? Save(AreaDTO toSave, string currentUserEmail)
        {
            using (var db = new DB())
            {
                try
                {
                    Area area = (from x in db.Area where x.AreaId == toSave.AreaId select x).FirstOrDefault();
                    if (area == null)
                    {
                        if (toSave.AreaId == Guid.Empty)
                        {
                            area = new Area();
                            area.AreaId = Guid.NewGuid();
                        }
                        else
                        {
                            area = new Area();
                            area.AreaId = toSave.AreaId;
                        }

                        area.CreatedDate = DateTime.Now;
                        area.CreatedBy = currentUserEmail;
                        db.Area.Add(area);
                    }

                    area.Name = toSave.Name;
                    area.DisplayOrder = toSave.DisplayOrder;
                    area.IsActive = toSave.IsActive;
                    area.UpdatedDate = DateTime.Now;
                    area.UpdatedBy = currentUserEmail;

                    // SubAreas
                    if (toSave.SubAreas != null && toSave.SubAreas.Any())
                    {
                        toSave.SubAreas
                            .ToList()
                            .ForEach(x => SubAreas.Save(x, currentUserEmail));
                    }

                    db.SaveChanges();

                    return area.AreaId;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }

        /// <summary>
        /// Toggles Area record
        /// </summary>
        /// <param name="id">ID</param>
        public static void Toggle(Guid id)
        {
            using (var db = new DB())
            {
                try
                {
                    var area = (from x in db.Area where x.AreaId == id select x).FirstOrDefault();
                    area.IsActive = !area.IsActive;
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }
    }
}
