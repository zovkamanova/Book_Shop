using BulkyBook.Models;
using Microsoft.AspNetCore.Mvc;
using BulkyBook.DataAccess;
using BulkyBook.DataAccess.Repository.IRepository;

namespace BulkyBookWeb.Controllers;
[Area("Admin")]

public class CategoryController : Controller
{
    private readonly IUnitOfWork _unitOfWork;

    public CategoryController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public IActionResult Index()
    {
        IEnumerable<Category> categoryList = _unitOfWork.Category.GetAll();
        return View(categoryList);
    }

    //Get
    public IActionResult Create()
    {      
        return View();
    }

    //Post
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Category category)
    {
        if (category.Name == category.DisplayOrder.ToString())
        {
            ModelState.AddModelError("Name", "The DisplayOrder cannot exactly matches the Name");
        }

        if (!ModelState.IsValid)
        {
            return View();
        }
        _unitOfWork.Category.Add(category);
        _unitOfWork.Save();
        TempData["success"] = "Category was created successfully!";
        return RedirectToAction("Index");
    }


    //Get
    public IActionResult Edit(int? id)
    {
        if(id==null || id == 0)
        {
            return NotFound();
        }

        var categoryFromDb = _unitOfWork.Category.GetFirstOrDefault(c=>c.Id==id);

        if (categoryFromDb == null)
        {
            return NotFound();
        }
        return View(categoryFromDb);
    }

    //Post
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(Category category)
    {
        if (category.Name == category.DisplayOrder.ToString())
        {
            ModelState.AddModelError("Name", "The DisplayOrder cannot exactly matches the Name");
        }

        if (!ModelState.IsValid)
        {
            return View();
        }
        _unitOfWork.Category.Update(category);
        _unitOfWork.Save();
        TempData["success"] = "Category was updated successfully!";
        return RedirectToAction("Index");
    }

    //Get
    public IActionResult Delete(int? id)
    {
        if (id == null || id == 0)
        {
            return NotFound();
        }

        var categoryFromDb = _unitOfWork.Category.GetFirstOrDefault(c=>c.Id==id);

        if (categoryFromDb == null)
        {
            return NotFound();
        }
        return View(categoryFromDb);
    }

    //Post
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult DeletePost(int? id)
    {
        var category = _unitOfWork.Category.GetFirstOrDefault(c=>c.Id==id);
        if (category != null)
        {
            _unitOfWork.Category.Remove(category);
            _unitOfWork.Save();
            TempData["success"] = "Category was deleted successfully!";
        } 
        return RedirectToAction("Index");
    }
}
