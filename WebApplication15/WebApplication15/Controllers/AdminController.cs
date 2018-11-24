using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication15.Models;
using System.Data.Entity;
using System.Data;
using System.Data.SqlClient;
namespace WebApplication15.Controllers
{
    public class AdminController : Controller
    {
        
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Student(string searchby, string search)
            {
            using (LMSEntities db = new LMSEntities())
            {
                if (searchby == "Name")
                {
                    return View(db.tbl_student.Where(x => x.Name.Contains(search) || search == null).ToList());
                }
                else
                {
                    return View(db.tbl_student.Where(x => x.Registration_Number.Contains(search) || search == null).ToList());
                }

            }

        }
        public ActionResult AddStudent()
        {
            return View();
        }
        public ActionResult IssuedBooks(string searchby, string search)
        {
            using (LMSEntities db = new LMSEntities())
            {
                if (searchby == "Number")
                {
                    return View(db.Issued_Books.Where(x => x.Number.Contains(search) || search == null).ToList());
                }
                else
                {
                    return View(db.Issued_Books.Where(x => x.Email.Contains(search) || search == null).ToList());
                }


            }

        }
        public ActionResult AddIssuedbook()
        {
            return View();
        }
        public int Fine(DateTime t1, DateTime t2)
        {
            TimeSpan t = t1.Subtract(t2);
            int  f = (int)t.TotalDays;
            return f * 50;
        }


        [HttpPost]
        public ActionResult AddIssuedbook(Issued_Books v)
        {
            try
            {
                IssuedBooksVM m = new IssuedBooksVM();
                m.Number = v.Number;
                m.Email = v.Email;
                m.Issue_date = v.Issue_date;
                m.Return_date = v.Return_date;
                m.Due_date = v.Due_date;
                v.Fine = Fine(v.Return_date, v.Due_date);
                m.Fine = v.Fine;
                m.Status = v.Status;

                LMSEntities db = new LMSEntities();
                db.Issued_Books.Add(v);

                db.SaveChanges();

                return RedirectToAction("IssuedBooks");
            }
            catch
            {
                return View();
            }
            
        }
        [HttpPost]
        public ActionResult AddStudent(tbl_student s, Email e)
        {
            try
            {
                Student st = new Student();
                st.Email = s.Email;
                e.Email1 = s.Email;
                st.Name = s.Name;
                st.Registration_Number = s.Registration_Number;
                LMSEntities db = new LMSEntities();
                db.tbl_student.Add(s);
                db.Emails.Add(e);
                db.SaveChanges();

                return RedirectToAction("Student");
            }
            catch
            {
                return View();
            }

        }
        public ActionResult Teacher(string searchby, string search)
        {
            using (LMSEntities db = new LMSEntities())
            {
                if(searchby == "Name")
                {
                    return View(db.tbl_teacher.Where(x=> x.Name.Contains(search) || search == null).ToList());
                }
                else 
                {
                    return View(db.tbl_teacher.Where(x => x.Designation.Contains(search) || search == null).ToList());
                }
               
              
                
            }
        }
        public ActionResult AddTeacher()
        {
            return View();
        }
        // GET: Admin/Details/5

      /*  public void addemail()
        {
            LibraryManagementSystemEntities db = new LibraryManagementSystemEntities();
            List<tbl_teacher> teachers = db.tbl_teacher.ToList();

            List<Email> lst = teachers.Select(x => new Email { Email1 = x.Email }).ToList();
            int count = lst.Count();

            int i = 1;
            while (i < count)
            {
                db.Emails.Add(lst[i]);
                ++i;
                db.SaveChanges();

            }
        }
        */
        [HttpPost]
        public ActionResult AddTeacher(tbl_teacher t, Email e)
        {
            try
            {
                LMSEntities DB = new LMSEntities();
                Teacher teacher = new Teacher();
                teacher.Name = t.Name;
                teacher.Email = t.Email;
                
               

                 e.Email1 = t.Email;
                teacher.Designation = t.Designation;

               
                DB.tbl_teacher.Add(t);
               DB.Emails.Add(e);
                
                DB.SaveChanges();


               

                return RedirectToAction("Teacher");
            }
            catch
            {
                return View();
            }
           
        }
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Admin/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Admin/Edit/5
        public ActionResult Edit(int id)
        {
            Teacher t = new Teacher();
            LMSEntities db = new LMSEntities();
            foreach(tbl_teacher T in db.tbl_teacher)
            {
                if(T.ID == id)
                {
                    t.Name = T.Name;
                    t.Email = T.Email;
                    t.Designation = T.Designation;
                    break;

                }
            }
           
            return View();
        }
        public ActionResult Editst(int id)
        {
            Student st = new Student();
            LMSEntities db = new LMSEntities();
            foreach (tbl_student ST in db.tbl_student)
            {
                if (ST.ID == id)
                {
                    st.Name = ST.Name;
                    st.Email = ST.Email;
                    st.Registration_Number = ST.Registration_Number;
                    break;

                }
            }

            return View();
        }
        [HttpPost]
        public ActionResult Editst(int id, Student t)
        {

            LMSEntities db = new LMSEntities();
                db.tbl_student.Find(id).Name = t.Name;
                db.tbl_student.Find(id).Email = t.Email;

                db.tbl_student.Find(id).Registration_Number = t.Registration_Number;
                db.SaveChanges();


                return RedirectToAction("Student");
            }

        // POST: Admin/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, Teacher t)
        {
            {
                LMSEntities db = new LMSEntities();
                db.tbl_teacher.Find(id).Name = t.Name;
                db.tbl_teacher.Find(id).Email = t.Email;
                
                db.tbl_teacher.Find(id).Designation = t.Designation;
                db.SaveChanges();


                return RedirectToAction("Teacher");
            }
            /*catch
            {
                return View();
            }*/
        }
        public ActionResult Deletest(int id)
        {
            LMSEntities db = new LMSEntities();
            foreach (tbl_student t1 in db.tbl_student)
            {
                if (t1.ID == id)
                {
                    db.tbl_student.Remove(t1);

                }
            }
            db.SaveChanges();




            return View("Teacher");
        }

        // GET: Admin/Delete/5
        public ActionResult Delete(int id)
        {
            LMSEntities db = new LMSEntities();
            foreach (tbl_teacher t1 in db.tbl_teacher)
            {
                if (t1.ID == id)
                {
                    db.tbl_teacher.Remove(t1);
                   
                }
            }
            db.SaveChanges();




            return View("Teacher");
        }

        // POST: Admin/Delete/5
        [HttpPost]
        public ActionResult Delete(string id, Teacher t)
        {
            try
            {
                return View();
               
            }
            catch
            {
                return View();
            }
        }
    }
}
