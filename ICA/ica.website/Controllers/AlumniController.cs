using AutoMapper;
using AutoMapper.QueryableExtensions;
using ica.website.Domain;
using ica.website.Filters;
using ica.website.Infrastructure;
using ica.website.Infrastructure.Alerts;
using ica.website.Models;
using ica.website.Models.Alumni;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;

namespace ica.website.Controllers
{
    [Authorize(Roles = "Web Maintenance")]
    public class AlumniController : AppFrameController
    {
        // GET: Alumni
       
        private readonly IBaseOperations _context;
        private readonly ICurrentUser _currentUser;

        public AlumniController(IBaseOperations context,
            ICurrentUser currentUser)
        {
            _context = context;
            _currentUser = currentUser;
        }
        public ActionResult Index()
        {
            ViewBag.selected = 1;
            var models = _context.GetSearchResult(new ConditionViewModel<ApplicationUser>()
            {
                ChangeOrderDirection = false,
                OrderDirection = "desc"
            }, d => d.UpdateDatetime, null, true);
            return View(models);
        }
        [HttpPost]
        public ActionResult Index(ConditionViewModel<ApplicationUser> input)
        {
            Expression <Func<ApplicationUser, bool>> searchName;
            if (string.IsNullOrEmpty(input.Search))
            {
                searchName = t => true;
            }
            else
            {
                searchName = t =>  t.UserName.Contains(input.Search) || t.Company.Contains(input.Search) || t.StudentId.Contains(input.Search)|| t.JobTitle.Contains(input.Search);
            }
            input.Func = searchName;
            var includes = new string[] { };
            switch (input.Order)
            {
                case "UserName":
                    return View(_context.GetSearchResult(input, d => d.UserName, includes, false));
                case "Company":
                    return View(_context.GetSearchResult(input, d => d.Company, includes, false));
                case "StudentId":
                    return View(_context.GetSearchResult(input, d => d.StudentId, includes, false));
                case "Position":
                    return View(_context.GetSearchResult(input, d => d.JobTitle, includes, false));
                case "Relevent":
                    return View(_context.GetSearchResult(input, d => d.Relevent, includes, false));
                default:
                    return View(_context.GetSearchResult(input, d => d.UpdateDatetime, includes, false));
            }
        }
        //public ActionResult Edit(string Id)
        //{
        //    return View(_context.FindDetail<ApplicationUser>(Id));
        //}
        [Log("Started to edit alumni {id}")]
        public ActionResult Edit(string id)
        {
            var form = _context.Get<ApplicationUser>(d=>d.Id==id).Project().To<UserViewModel>().SingleOrDefault();
            if (form == null)
            {
                return RedirectToAction<HomeController>(c => c.Index()).WithError("Unable to find the alumni.");
            }
            return View(form);
        }
        [HttpPost, ValidateAntiForgeryToken,ValidateInput(false)]
        public ActionResult Edit(UserViewModel form)
        {
            if (!ModelState.IsValid)
            {
                return View(form);
            }
            //Create the mapping between AutomobileDM to CarVM
            Mapper.CreateMap<UserViewModel, ApplicationUser>();
            //Perform the conversion and fetch the destination view model
            var updateAlumni = Mapper.Map<ApplicationUser>(form);
            updateAlumni.UpdateDatetime = DateTime.Now;
            _context.UpdateOneEntity<ApplicationUser>(updateAlumni);
            //throw new InvalidOperationException();
            return RedirectToAction<AlumniController>(c => c.Index()).WithInfo("Alumni updated!");
        }
        public ActionResult Add()
        {
            return View(new AddUserViewModel());
        }
        [HttpPost, ValidateAntiForgeryToken, Log("Created Alumni")]
        public ActionResult Add(AddUserViewModel form)
        {
            if (!ModelState.IsValid)
            {
                return View(form);
            }
            //Create the mapping between AutomobileDM to CarVM
            Mapper.CreateMap<AddUserViewModel, ApplicationUser>();
            //Perform the conversion and fetch the destination view model
            var newAlumni = Mapper.Map<ApplicationUser>(form);

            newAlumni.UpdateDatetime = DateTime.Now;
            if(string.IsNullOrEmpty(form.HeaderImage))
            newAlumni.HeaderImage = "/images/alumni/default_profile.png";
            try
            {
                _context.AddOneEntity<ApplicationUser>(newAlumni);
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "Please check if your student name and id are same with others");
                return View(form);
            }
            return RedirectToAction<AlumniController>(c => c.Index()).WithInfo("Alumni created!");
        }
        public ActionResult Delete(string id)
        {
            _context.DeleteOneEntity<ApplicationUser>(id);
            return RedirectToAction<AlumniController>(c => c.Index()).WithInfo("Alumni deleted!");
        }
        public ActionResult AlumniPublic()
        {
            var models = _context.GetSearchResult(new ConditionViewModel<ApplicationUser>()
            {
                ChangeOrderDirection = false,
                OrderDirection = "desc",
                Func = d => d.Public
            }, d => d.UpdateDatetime, null, true);
            foreach (var item in models.Data)
            {
                var des = item.Description;
                if (des == null) continue;
                var start = des.IndexOf("<p>");
                var end = des.IndexOf("</p>");
                if(start==-1 || end== -1){
                    continue;
                }
                item.Description = des.Substring(start, end - start);
            }
           
            return View(models);

        }
        [HttpPost]
        public ActionResult AlumniPublic(ConditionViewModel<ApplicationUser> input)
        {
            Expression<Func<ApplicationUser, bool>> searchName;
            if (string.IsNullOrEmpty(input.Search))
            {
                searchName = t => t.Public;
            }
            else
            {
                searchName = t => t.Public &&( t.UserName.Contains(input.Search) || t.Company.Contains(input.Search) || t.StudentId.Contains(input.Search) || t.JobTitle.Contains(input.Search));
            }
            input.Func = searchName;
            var includes = new string[] { };
            ConditionViewModel<ApplicationUser> models=new ConditionViewModel<ApplicationUser>();
            switch (input.Order)
            {
                case "UserName":
                    models=_context.GetSearchResult(input, d => d.UserName, includes, false);
                    break;
                case "Company":
                    models = _context.GetSearchResult(input, d => d.Company, includes, false);
                    break;
                case "StudentId":
                    models = _context.GetSearchResult(input, d => d.StudentId, includes, false);
                    break;
                case "Position":
                    models = _context.GetSearchResult(input, d => d.JobTitle, includes, false);
                    break;
                case "Relevent":
                    models = _context.GetSearchResult(input, d => d.Relevent, includes, false);
                    break;
                default:
                    models = _context.GetSearchResult(input, d => d.UpdateDatetime, includes, false);
                    break;
            }
            foreach (var item in models.Data)
            {
                var des = item.Description;
                if (des == null) continue;
                var start = des.IndexOf("<p>");
                var end = des.IndexOf("</p>");
                if (start == -1 || end == -1)
                {
                    continue;
                }
                item.Description = des.Substring(start, end - start);
            }
            return View(models);
        }
        public ActionResult GetDetail(string Id)
        {
            var temp = _context.FindDetail<ApplicationUser>(Id);
            return PartialView("_ModalDetail",temp);
        }
        public ActionResult ImportData(FormCollection forms)
        {
            HttpPostedFileBase hpf = Request.Files[0] as HttpPostedFileBase;
            string fileName = DateTime.Now.Date.ToShortDateString().Replace("/", "_") + Path.GetFileName(hpf.FileName);
            string extension = Path.GetExtension(hpf.FileName);
            if (extension != ".xls") return Json("error occured", JsonRequestBehavior.AllowGet);
            string savedFileName = Path.Combine(Server.MapPath("~/UploadFiles"), fileName);
            hpf.SaveAs(savedFileName);

            int newStatementId = ImportDataToDatabase(savedFileName, hpf.FileName);
            //if (type == "picture")
            //    db.Pictures.Find(id).Path = "/images/" + fileName;
            if (newStatementId > 0)
                return Json("done", JsonRequestBehavior.AllowGet);
            else
                return Json("error occured", JsonRequestBehavior.AllowGet);
        }

        public int ImportDataToDatabase(String path, string fileName)
        {
            try
            {
                HSSFWorkbook hssfwb;
                using (var file = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    hssfwb = new HSSFWorkbook(file);
                }
                ISheet sheet = hssfwb.GetSheet("sheet1");
                int row = 1;
                // 1 studentId,studentName,3 department, 4 emailaddress, 5 phonenumber, 6 jobStatus, 7 jobType, 8 jobTitle, 9 company
                // 10 further study, 11 institute name, 12 country, 13 remark
                while (sheet.GetRow(row) != null)
                {
                    //student id
                    if (sheet.GetRow(row).GetCell(0) == null)
                        break;
                    var studentId = sheet.GetRow(row).GetCell(0).StringCellValue;
                    if (_context.Get<ApplicationUser>(d => d.StudentId == studentId).Any())
                    {
                        row++;
                        continue;
                    }
                        
                    //student name
                    if (sheet.GetRow(row).GetCell(1) == null)
                        break;
                    var studentName = sheet.GetRow(row).GetCell(1).StringCellValue;
                    if (_context.Get<ApplicationUser>(d => d.UserName == studentName).Any())
                    {
                        row++;
                        continue;
                    }
                    //3 department
                    string department = null;
                    if (sheet.GetRow(row).GetCell(2) != null)
                        department= sheet.GetRow(row).GetCell(2).StringCellValue;
                    //4 emailaddress
                    string emailaddress = null;
                    if (sheet.GetRow(row).GetCell(3) != null)
                        emailaddress = sheet.GetRow(row).GetCell(3).StringCellValue;
                    //5 phonenumber
                    string phonenumber = null;
                    if (sheet.GetRow(row).GetCell(4) != null)
                        phonenumber = sheet.GetRow(row).GetCell(4).StringCellValue;
                    //6 jobStatus
                    string jobStatus = null;
                    if (sheet.GetRow(row).GetCell(5) != null)
                        jobStatus = sheet.GetRow(row).GetCell(5).StringCellValue;
                    var jobStatusValue = JobStatus.Other;
                    if(jobStatus==null)
                        jobStatusValue = JobStatus.Other;
                    else
                    {
                        if (jobStatus.ToLower() == "part time")
                            jobStatusValue = JobStatus.PartTime;
                        else
                        if (jobStatus.ToLower() == "full time")
                            jobStatusValue = JobStatus.FullTime;
                        else
                            jobStatusValue = JobStatus.Other;
                    }
                    //7 jobType, 
                    string jobType = null;
                    if (sheet.GetRow(row).GetCell(6) != null)
                        jobType = sheet.GetRow(row).GetCell(6).StringCellValue;
                    long jobTypeVlaue = 1;

                    if (jobType != null)
                    {
                        var type=_context.Get<WebJobType>(d => d.Name == jobType).FirstOrDefault();
                        if (type!=null)
                            jobTypeVlaue = type.Id;
                    }
                   
                    //8 jobTitle, 
                    string jobTitle = null;
                    if (sheet.GetRow(row).GetCell(7) != null)
                        jobTitle = sheet.GetRow(row).GetCell(7).StringCellValue;
                    //9 company
                    string company = null;
                    if (sheet.GetRow(row).GetCell(8) != null)
                        company = sheet.GetRow(row).GetCell(8).StringCellValue;
                    // 10 further study, 
                    string further = null;
                    if (sheet.GetRow(row).GetCell(9) != null)
                        further = sheet.GetRow(row).GetCell(9).StringCellValue;
                    //11 institute
                    string institute = null;
                    if (sheet.GetRow(row).GetCell(10) != null)
                        institute = sheet.GetRow(row).GetCell(10).StringCellValue;
                    // 12 country
                    string country = null;
                    if (sheet.GetRow(row).GetCell(11) != null)
                        country = sheet.GetRow(row).GetCell(11).StringCellValue;
                    var countryEntity = _context.Get<WebCountry>(d => d.Name == country).FirstOrDefault();
                    int? countryId = null;
                    if (countryEntity != null)
                    {
                        countryId = countryEntity.Id;
                    }
                    //13 remark
                    string remark = null;
                    if (sheet.GetRow(row).GetCell(12) != null)
                        remark = sheet.GetRow(row).GetCell(12).StringCellValue;
                    
                    // 1 studentId,studentName,3 department, 4 emailaddress, 5 phonenumber, 6 jobStatus, 7 jobType, 8 jobTitle, 9 company
                    // 10 further study, 11 institute name, 12 country, 13 remark
                    _context.AddOneEntity<ApplicationUser>(new ApplicationUser
                    {
                        StudentId=studentId,
                        UserName=studentName,
                        Department=department,
                        Email=emailaddress,
                        PhoneNumber=phonenumber,
                        JobStatus= jobStatusValue,
                        JobTypeId= jobTypeVlaue,
                        JobTitle=jobTitle,
                        Company=company,
                        NewInstituteName=institute,
                        CountryId=countryId,
                        Remark=remark,
                        UpdateDatetime=DateTime.Now,
                        Public=false
                    });
                    row++;
                }
                return 1;
            }
            catch (Exception e)
            {
                return -1;
            }
        }
        public ActionResult UploadImage(FormCollection form)
        {
            HttpPostedFileBase file = Request.Files[0] as HttpPostedFileBase;
            var alumniId = form.GetValues("alumniId")[0];
            string pic = "";
            if (file != null)
            {
                var fileName = Path.GetFileName(file.FileName);
                var ext = fileName.Substring(fileName.IndexOf(".") + 1).ToLower();
                if (ext != "gif" && ext == "png" && ext == "jpg" && ext == "bmp")
                {
                    return Json("Sorry, you can only upload gif,png,jpg or bmp file.", JsonRequestBehavior.AllowGet);
                }
                if (file.ContentLength > 200000)//200k
                {
                    return Json("Sorry, your logo file size can not be bigger than 200k. Please upload proper size file.", JsonRequestBehavior.AllowGet);
                }
                pic = Guid.NewGuid().ToString() + "." + ext;
                string path = System.IO.Path.Combine(Server.MapPath("/images/Alumni/"), pic);
                // file is uploaded
                file.SaveAs(path);
                return Json("/images/Alumni/" + pic, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("alert('Please select your logo file.')", JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult tempAdd()
        {
            return View();
        }
    }
}