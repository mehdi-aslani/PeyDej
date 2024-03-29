﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using PeyDej.Data;
using PeyDej.Models;
using PeyDej.Models.Bases;
using PeyDej.Models.Dtos;

namespace PeyDej.Controllers
{
    [Authorize]
    public class MachineController : Controller
    {
        private readonly PeyDejContext _context;

        public MachineController(PeyDejContext context)
        {
            _context = context;
        }

        // GET: Machine
        public IActionResult Index()
        {
            return View(_context.Machines.Where(m => m.GeneralStatusId == GeneralStatus.Active).AsEnumerable());
        }

        // GET: Machine/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var machine = await _context.Machines
                .FirstOrDefaultAsync(m => m.Id == id);
            if (machine == null)
            {
                return NotFound();
            }

            return View(machine);
        }

        // GET: Machine/Create
        public IActionResult Create()
        {
            var data = new Machine()
            {
                Department = null,
                Process = null,
                Motors = _context.Motors.Where(w => w.GeneralStatusId != GeneralStatus.Deleted).AsEnumerable(),
                SpareParts = _context.SpareParts.Where(w => w.GeneralStatusId != GeneralStatus.Deleted).AsEnumerable(),
                DepartmentIds = _context.VwCategories.Where(w => w.CategoryId == 2).Select(s => new CategoryDto()
                {
                    Id = s.SubCategoryId,
                    Name = s.SubCategoryCaption
                }).AsEnumerable(),
                ProcessIds = _context.VwCategories.Where(w => w.CategoryId == 3).Select(s => new CategoryDto()
                {
                    Id = s.SubCategoryId,
                    Name = s.SubCategoryCaption
                }).AsEnumerable()
            };
            return View(data);
        }

        // POST: Machine/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Machine machine)
        {
            if (ModelState.IsValid)
            {
                var result = _context.Add(machine);
                await _context.SaveChangesAsync();

                foreach (var motorId in machine.MotorIds)
                {
                    _context.MachineMotors.Add(new MachineMotor(machineId: machine.Id, motorId));
                    _context.SaveChanges();
                }
                foreach (var sparePartId in machine.SparePartIds)
                {
                    _context.SparePartMachines.Add(new SparePartMachine(machineId: machine.Id, sparePartId));
                    _context.SaveChanges();
                }
                return RedirectToAction(nameof(Index));
            }

          
            machine.Motors = _context.Motors.Where(w => w.GeneralStatusId != GeneralStatus.Deleted).AsEnumerable();
            machine.SpareParts = _context.SpareParts.Where(w => w.GeneralStatusId != GeneralStatus.Deleted).AsEnumerable();
            machine.DepartmentIds = _context.VwCategories.Where(w => w.CategoryId == 2)
                .Select(s => new CategoryDto() { Id = s.SubCategoryId, Name = s.SubCategoryCaption }).AsEnumerable();
            machine.ProcessIds = _context.VwCategories.Where(w => w.CategoryId == 3)
                .Select(s => new CategoryDto() { Id = s.SubCategoryId, Name = s.SubCategoryCaption }).AsEnumerable();
            return View(machine);
        }

        // GET: Machine/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var machine = await _context.Machines.FindAsync(id);
            if (machine == null)
            {
                return NotFound();
            }

            machine.MotorIds = _context.MachineMotors
                .Where(w => w.MachineId == id).Select(s => s.MotorId).ToList();

            machine.SparePartIds = _context.SparePartMachines
                .Where(w => w.MachineId == id).Select(s => s.SparePartId).ToList();

            machine.Motors = _context.Motors.Where(w => w.GeneralStatusId != GeneralStatus.Deleted).AsEnumerable();
            machine.SpareParts = _context.SpareParts.Where(w => w.GeneralStatusId != GeneralStatus.Deleted).AsEnumerable();
            machine.DepartmentIds = _context.VwCategories.Where(w => w.CategoryId == 2).Select(s => new CategoryDto()
            {
                Id = s.SubCategoryId,
                Name = s.SubCategoryCaption
            }).AsEnumerable();
            machine.ProcessIds = _context.VwCategories.Where(w => w.CategoryId == 3).Select(s => new CategoryDto()
            {
                Id = s.SubCategoryId,
                Name = s.SubCategoryCaption
            }).AsEnumerable();


            return View(machine);
        }

        // POST: Machine/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, Machine machine)
        {
            if (id != machine.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(machine);
                    await _context.SaveChangesAsync();



                    _context.MachineMotors.Where(w => w.MachineId == machine.Id).ExecuteDelete();
                    foreach (var l in machine.MotorIds)
                    {
                        _context.MachineMotors.Add(new MachineMotor(machineId: machine.Id, l));
                        _context.SaveChanges();
                    }
                    _context.SparePartMachines.Where(w => w.MachineId == machine.Id).ExecuteDelete();
                    foreach (var l in machine.SparePartIds)
                    {
                        var t = _context.SparePartMachines.Add(new SparePartMachine(machineId: machine.Id, l));
                        _context.SaveChanges();
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MachineExists(machine.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction(nameof(Index));
            }

            machine.Motors = _context.Motors.Where(w => w.GeneralStatusId != GeneralStatus.Deleted).AsEnumerable();
            machine.SpareParts = _context.SpareParts.Where(w => w.GeneralStatusId != GeneralStatus.Deleted).AsEnumerable();
               machine.DepartmentIds = _context.VwCategories.Where(w => w.CategoryId == 2).Select(s => new CategoryDto()
            {
                Id = s.SubCategoryId,
                Name = s.SubCategoryCaption
            }).AsEnumerable();
            machine.ProcessIds = _context.VwCategories.Where(w => w.CategoryId == 3).Select(s => new CategoryDto()
            {
                Id = s.SubCategoryId,
                Name = s.SubCategoryCaption
            }).AsEnumerable();
            return View(machine);
        }

        // POST: Machine/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var machine = await _context.Machines.FindAsync(id);
            if (machine != null)
            {
                machine.GeneralStatusId = GeneralStatus.Deleted;
                _context.Machines.Update(machine);
            }

            await _context.SaveChangesAsync();
            return Json(new { hasError = false, message = "" });
        }

        private bool MachineExists(long id)
        {
            return (_context.Machines?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}