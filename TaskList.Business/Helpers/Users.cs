using System;
using System.Collections.Generic;
using System.Linq;
using TaskList.Core.DTOs;
using TaskList.Core.Exceptions;
using TaskList.Data.Model;

namespace TaskList.Business.Helpers
{
    public static class Users
    {
        public static UserDTO GetByID(Guid id)
        {
            using (var db = new DB())
            {
                var dto = db.User
                    .Where(x => x.UserId == id)
                    .Select(x => new UserDTO()
                    {
                        UserId = x.UserId,
                        FirstName = x.FirstName,
                        LastName = x.LastName,
                        Name = x.LastName + ", " + x.FirstName,
                        Email = x.Email,
                        IsSupervisor = x.IsSupervisor,
                        SupervisorId = x.SupervisorId,
                        IsActive = x.IsActive,
                    })
                    .FirstOrDefault();

                return dto;
            }
        }

        public static List<UserDTO> GetAll(bool activeOnly = false, bool supervisorsOnly = false)
        {
            using (var db = new DB())
            {
                IQueryable<User> query = db.User;

                if (activeOnly)
                {
                    query = query.Where(x => x.IsActive);
                }

                if (supervisorsOnly)
                {
                    query = query.Where(x => x.IsSupervisor);
                }

                var dtos = query
                    .Select(x => new UserDTO()
                    {
                        UserId = x.UserId,
                        FirstName = x.FirstName,
                        LastName = x.LastName,
                        Email = x.Email,
                        IsSupervisor = x.IsSupervisor,
                        SupervisorId = x.SupervisorId,
                        IsActive = x.IsActive,
                        SupervisorName = Users.GetSupervisorName(x.SupervisorId)
                    });
                
                return dtos
                    .OrderBy(x => x.LastName)
                    .ThenBy(x => x.FirstName)
                    .ToList();
            }
        }

        public static Guid? Save(UserDTO toSave, string currentUserEmail)
        {
            if(toSave.SupervisorId != null)
            {
                var supervisor = Users.GetByID((Guid) toSave.SupervisorId);

                if (supervisor == null || !supervisor.IsSupervisor)
                {
                    throw new EndUserException("Selected supervisor is not a supervisor.");
                }
            }

            using (var db = new DB())
            {
                try
                {
                    User entity = db.User
                        .Where(x => x.UserId == toSave.UserId)
                        .FirstOrDefault();

                    if(entity == null)
                    {
                        if(toSave.UserId == Guid.Empty)
                        {
                            entity = new User();
                            entity.UserId = Guid.NewGuid();
                        }
                        else
                        {
                            entity = new User();
                            entity.UserId = toSave.UserId;
                        }

                        entity.CreatedDate = DateTime.Now;
                        entity.CreatedBy = currentUserEmail;
                        db.User.Add(entity);
                    }

                    entity.FirstName = toSave.FirstName;
                    entity.LastName = toSave.LastName;
                    entity.Email = toSave.Email;
                    entity.IsSupervisor = toSave.IsSupervisor;
                    entity.SupervisorId = toSave.SupervisorId;
                    entity.IsActive = toSave.IsActive;
                    entity.UpdatedDate = DateTime.Now;
                    entity.UpdatedBy = currentUserEmail;
                    db.SaveChanges();

                    return entity.UserId;
                }
                catch(Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }

        public static void Toggle(Guid id)
        {
            using(var db = new DB())
            {
                try
                {
                    User entity = db.User
                        .Where(x => x.UserId == id)
                        .FirstOrDefault();

                    entity.IsActive = !entity.IsActive;
                    db.SaveChanges();
                }
                catch(Exception ex)
                {
                    throw new Exception(ex.Message);               
                }                
            }
        }

        private static string GetSupervisorName(Guid? id)
        {
            if(id == null || id == Guid.Empty)
            {
                return null;
            }

            UserDTO supervisor = Users.GetByID((Guid) id);

            return supervisor.LastName + ", " + supervisor.FirstName;
        }

        public static UserDTO GetByEmail(string email)
        {
            using (var db = new DB())
            {
                var dto = db.User
                    .Where(x => x.Email == email)
                    .Select(x => new UserDTO()
                    {
                        UserId = x.UserId,
                        FirstName = x.FirstName,
                        LastName = x.LastName,
                        Email = x.Email,
                        IsSupervisor = x.IsSupervisor,
                        SupervisorId = x.SupervisorId,
                        IsActive = x.IsActive,
                    })
                    .FirstOrDefault();

                return dto;
            }
        }
    }
}
