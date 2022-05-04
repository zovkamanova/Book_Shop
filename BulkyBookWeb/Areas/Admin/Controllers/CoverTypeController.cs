using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBookWeb.Areas.Admin.Controllers;
[Area("Admin")]

public class CoverTypeController : Controller
{
    private readonly IUnitOfWork _unitOfWork;
    public CoverTypeController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public IActionResult Index()
    {
        IEnumerable<CoverType> coverTypeList = _unitOfWork.CoverType.GetAll();
        return View(coverTypeList);
    }
    public IActionResult Create()
    {
        return View();
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(CoverType coverType)
    {
        if (!ModelState.IsValid)
        {
            return View();
        }
        _unitOfWork.CoverType.Add(coverType);
        _unitOfWork.Save();
        TempData["success"] = "Cover type was created successfully!";
        return RedirectToAction("Index");
    }
    public IActionResult Edit(int? id)
    {
        if (id == null || id == 0)
        {
            return NotFound();
        }
        var coverTypeFromDb = _unitOfWork.CoverType.GetFirstOrDefault(c => c.Id == id);
        if (coverTypeFromDb == null)
        {
            return NotFound();
        }
        return View(coverTypeFromDb);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(CoverType coverType)
    {
        if (!ModelState.IsValid)
        {
            return View();
        }
        _unitOfWork.CoverType.Update(coverType);
        _unitOfWork.Save();
        TempData["success"] = "Cover type was updated successfully!";
        return RedirectToAction("Index");
    }
    public IActionResult Delete(int? id)
    {
        if(id==null|| id == 0)
        {
            return NotFound();
        }
        var coverTypeFromDb = _unitOfWork.CoverType.GetFirstOrDefault(c => c.Id == id);
        return View(coverTypeFromDb);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Delete(CoverType coverType)
    {
        if (coverType == null)
        {
            return NotFound();
        }
        _unitOfWork.CoverType.Remove(coverType);
        _unitOfWork.Save();
        TempData["success"] = "Cover type was deleted successfully!";
        return RedirectToAction("Index");
    }
}
