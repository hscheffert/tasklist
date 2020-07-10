using System;
using System.Collections.Generic;
using System.Linq;
using TaskList.Core.DTOs;
using TaskList.Data.Model;

namespace TaskList.Business.Helpers
{
    public static class SubAreas
    {
        public static SubAreaDTO GetByID(Guid id)
        {
            using (var db = new DB())
            {
                var dto = db.SubArea
                    .Where(x => x.SubAreaId == id)
                    .Select(x => new SubAreaDTO()
                    {
                        SubAreaId = x.SubAreaId,
                        AreaId = x.AreaId,
                        Name = x.Name,
                        AreaName = x.Area.Name,
                        DisplayOrder = x.DisplayOrder,
                        IsActive = x.IsActive,
                        CreatedBy = x.CreatedBy,
                        CreatedDate = x.CreatedDate,
                        UpdatedBy = x.UpdatedBy,
                        UpdatedDate = x.UpdatedDate,
                    })
                    .FirstOrDefault();

                return dto;
            }
        }

        public static List<SubAreaDTO> GetAll(bool activeOnly = false)
        {
            using (var db = new DB())
            {
                IQueryable<SubArea> query = db.SubArea;

                if (activeOnly)
                {
                    query = query.Where(x => x.IsActive);
                }

                var dtos = query
                    .Select(x => new SubAreaDTO()
                    {
                        SubAreaId = x.SubAreaId,
                        AreaId = x.AreaId,
                        Name = x.Name,
                        AreaName = x.Area.Name,
                        DisplayOrder = x.DisplayOrder,
                        IsActive = x.IsActive,
                        CreatedBy = x.CreatedBy,
                        CreatedDate = x.CreatedDate,
                        UpdatedBy = x.UpdatedBy,
                        UpdatedDate = x.UpdatedDate,
                    });

                return dtos
                    .OrderBy(x => x.DisplayOrder)
                    .ToList();
            }
        }

        public static List<SubAreaDTO> GetAllByAreaId(Guid areaId)
        {
            using (var db = new DB())
            {
                var dtos = db.SubArea
                    .Where(x => x.AreaId == areaId)
                    .Select(x => new SubAreaDTO()
                    {
                        SubAreaId = x.SubAreaId,
                        AreaId = x.AreaId,
                        Name = x.Name,
                        AreaName = x.Area.Name,
                        DisplayOrder = x.DisplayOrder,
                        IsActive = x.IsActive,
                        CreatedBy = x.CreatedBy,
                        CreatedDate = x.CreatedDate,
                        UpdatedBy = x.UpdatedBy,
                        UpdatedDate = x.UpdatedDate,
                    });

                return dtos
                    .OrderBy(x => x.DisplayOrder)
                    .ToList();
            }
        }

        public static Guid? Save(SubAreaDTO toSave, string currentUserEmail)
        {
            using (var db = new DB())
            {
                try
                {
                    SubArea entity = db.SubArea
                        .Where(x => x.SubAreaId == toSave.SubAreaId)
                        .FirstOrDefault();

                    if (entity == null)
                    {
                        if (toSave.SubAreaId == Guid.Empty)
                        {
                            entity = new SubArea();
                            entity.SubAreaId = Guid.NewGuid();
                        }
                        else
                        {
                            entity = new SubArea();
                            entity.SubAreaId = toSave.SubAreaId;
                        }

                        entity.CreatedDate = DateTime.Now;
                        entity.CreatedBy = currentUserEmail;
                        db.SubArea.Add(entity);
                    }

                    entity.Name = toSave.Name;
                    entity.AreaId = toSave.AreaId;
                    entity.DisplayOrder = toSave.DisplayOrder;
                    entity.IsActive = toSave.IsActive;
                    entity.UpdatedDate = DateTime.Now;
                    entity.UpdatedBy = currentUserEmail;
                    db.SaveChanges();

                    return entity.SubAreaId;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }

        public static void Toggle(Guid id)
        {
            using (var db = new DB())
            {
                try
                {
                    SubArea entity = db.SubArea
                        .Where(x => x.SubAreaId == id)
                        .FirstOrDefault();

                    entity.IsActive = !entity.IsActive;
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
