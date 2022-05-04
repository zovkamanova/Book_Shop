using BulkyBook.DataAccess;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BulkyBookWeb.Controllers;
[Area("Admin")]

public class CompanyController : Controller
{
    private readonly IUnitOfWork _unitOfWork;
   

    public CompanyController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public IActionResult Index()
    {
        IEnumerable<Company> companies = _unitOfWork.Company.GetAll();
        return View(companies);
    }

    //GET
    public IActionResult Upsert(int? id)
    {
        Company company = new Company();

        if (id == null || id == 0)
        {
            return View(company);
        }
        else
        {
            company = _unitOfWork.Company.GetFirstOrDefault(u => u.Id == id);
            return View(company);
        }


    }

    //POST
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Upsert(Company company)
    {

        if (ModelState.IsValid)
        {
            
            if (company.Id == 0)
            {
                _unitOfWork.Company.Add(company);
                TempData["success"] = "Company created successfully";
            }
            else
            {
                _unitOfWork.Company.Update(company);
                TempData["success"] = "Company updated successfully";
            }
            _unitOfWork.Save();
            
            return RedirectToAction("Index");
        }
        return View(company);
    }

    public IActionResult Delete(int? id)
    {
        if (id == null || id == 0)
        {
            return NotFound();
        }
        var companyFromDb = _unitOfWork.Company.GetFirstOrDefault(c => c.Id == id);
        return View(companyFromDb);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Delete(Company company)
    {
        if (company == null)
        {
            return NotFound();
        }
        _unitOfWork.Company.Remove(company);
        _unitOfWork.Save();
        TempData["success"] = "Company was deleted successfully!";
        return RedirectToAction("Index");
    }

}