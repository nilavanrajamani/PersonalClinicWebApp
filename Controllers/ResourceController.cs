using HtmlAgilityPack;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using PersonalWebsiteWebAppClinic.Data;
using PersonalWebsiteWebAppClinic.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace PersonalWebsiteWebAppClinic.Controllers
{
    public class ResourceController : Controller
    {
        private IDataContextCatalog _contextCatalog;
        public ResourceController()
        {
            _contextCatalog = new DataContextCatalog();
        }
        public ActionResult GetAllResources()
        {
            List<ResourceEntity> resourceEntities = _contextCatalog.ResourceEntityRepo.GetByPredicate(x => x.Title != null)
                .AsEnumerable().OrderByDescending(x => x.DateOfEntry).ToList();
            return View(resourceEntities);
        }
        public ActionResult RenderResourceEditView(string id)
        {
            ResourceEntity resourceEntity = null;
            if (!string.IsNullOrWhiteSpace(id))
            {
                resourceEntity = GetResourceById(id);
            }
            return View(resourceEntity);
        }
        public ActionResult GetResource(string id)
        {
            ResourceEntity resourceEntity = GetResourceById(id);
            return View(resourceEntity);
        }
        private ResourceEntity GetResourceById(string id)
        {
            IMongoQuery mongoQuery = Query<ResourceEntity>.EQ(p => p.Id, new MongoDB.Bson.ObjectId(id));
            ResourceEntity resourceEntity = _contextCatalog.ResourceEntityRepo.FindOne(mongoQuery);
            return resourceEntity;
        }
        [HttpPost]
        public JsonResult AddUpdateResource(string title, string htmlContentToSave, string resourceId)
        {
            ResultEntity resultEntity = new ResultEntity();

            //Create new resource entity
            ResourceEntity resourceEntity = null;

            try
            {
                if (!string.IsNullOrWhiteSpace(resourceId))
                {
                    resourceEntity = GetResourceById(resourceId);
                    if (resourceEntity != null)
                    {
                        resourceEntity.HtmlDocument = htmlContentToSave;
                        resourceEntity.Title = title;
                        resourceEntity.DateOfEntry = DateTime.Now;

                        _contextCatalog.ResourceEntityRepo.Update(resourceEntity);
                        resultEntity.Message = "Resource modified successfully";
                    }
                }
                else
                {
                    resourceEntity = new ResourceEntity();
                    resourceEntity.HtmlDocument = htmlContentToSave;
                    resourceEntity.Title = title;
                    resourceEntity.DateOfEntry = DateTime.Now;

                    //Repository 
                    _contextCatalog.ResourceEntityRepo.Add(resourceEntity);
                    resultEntity.Message = "Resource added successfully";
                }
                resultEntity.IsSuccess = true;
                //Return Json of added entity                
            }
            catch (Exception ex)
            {
                resultEntity.IsSuccess = false;
                resultEntity.Message = ex.Message;
            }
            return Json(resultEntity, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ViewAllCategories()
        {
            List<MainCategory> mainCategories = _contextCatalog.MainCategoryRepo.GetByPredicate(x => x.CategoryName != null)
                .AsEnumerable().OrderBy(x => x.Order).ToList();
            return View(mainCategories);
        }
    }
}
