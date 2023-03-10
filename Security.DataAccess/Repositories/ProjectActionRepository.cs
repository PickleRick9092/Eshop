using Security.DataAccessServiceContract.Repositories;
using Security.Domain.DTO.ProjectAction;
using Security.Domain.DTO.ProjectController;
using Security.Domain.Models;
using Shopping.DomainModel.BaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Security.DataAccess.Repositories
{
    public class ProjectActionRepository : IProjectActionRepository
    {

        private readonly SecurityContext db;
        public ProjectActionRepository(SecurityContext db)
        {
            this.db = db;
        }

        public OperationResult Add(ProjectActionAddModel model)
        {
            OperationResult op = new OperationResult("Add", "ProjectActions");
            {
                try
                {
                    var pa = new ProjectAction
                    {
                        ProjectActionName = model.ProjectActionName,
                        PersianTitle = model.PersianTitle,

                    };
                    db.Add(pa);
                    db.SaveChanges();
                    op.ToSuccess(pa.ProjectActionID, "Add Successfully!!!!!!!!!");

                }
                catch (Exception ex)
                {

                    op.ToFail("Add Failed :(" + ex.Message);

                }
                return op;
            };
        }

        public ProjectAction Get(int id)
        {
            return db.projectActions.FirstOrDefault(x => x.ProjectActionID == id);
        }

        public List<ProjectAction> GetAll()
        {
            return db.projectActions.ToList();
        }

        public OperationResult Remove(int id)
        {
            OperationResult op = new OperationResult("Remove", "ProjectActions", id);
            {
                try
                {
                    var pa = db.projectActions.FirstOrDefault(x => x.ProjectActionID == id);
                    db.projectActions.Remove(pa);
                    db.SaveChanges();
                    op.ToSuccess(id, "Remove Successfully");
                }
                catch (Exception ex)
                {

                    op.ToFail(id, "Delete Failed!" + ex.Message);
                }
                return op;
            }

        }


        public List<ProjectActionListItem> Search(ProjectActionSearchModel sm, out int RecordCount)
        {
            var q = from item in db.projectActions select item;
            if (!string.IsNullOrEmpty(sm.ProjectActionName))
            {
                q = q.Where(x => x.ProjectActionName.StartsWith(sm.ProjectActionName));
            }
            if (!string.IsNullOrEmpty(sm.PersianTitle))
            {
                q = q.Where(x => x.PersianTitle.StartsWith(sm.PersianTitle));
            }
            RecordCount = q.Count();
            return q.Select(x => new ProjectActionListItem
            {
                ProjectActionName = x.ProjectActionName,
                ProjectActionID = x.ProjectActionID,
                PersianTitle = x.PersianTitle,

            }
            ).OrderByDescending(x => x.ProjectActionID).Skip(sm.PageIndex * sm.PageSize).Take(sm.PageSize).ToList();
        }

        public OperationResult Update(ProjectActionUpdateModel model)
        {
            OperationResult op = new OperationResult("Update", "ProjectActions");
            try
            {
                var pa = Get(model.ProjectActionID);
                pa.ProjectActionName = model.ProjectActionName;
                pa.PersianTitle = model.PersianTitle;


                db.SaveChanges();
                op.ToSuccess("Update Successfully");
            }
            catch (Exception ex)
            {
                op.ToFail("Update Failed!" + ex.Message);
            }
            return op;
        }

        public bool ExistProjectActionName(string ProjectActionName)
        {
            return db.projectActions.Any(x => x.ProjectActionName == ProjectActionName);
        }

        public List<ProjectControllerDrop> PcDrops()
        {
            var q = from item in db.projectControllers select item;
            return q.Select(x => new ProjectControllerDrop
            {
                ProjectControllerID = x.ProjectControllerID,
                ProjectControllerName = x.ProjectControllerName
            }).ToList();

        }
    }
}
