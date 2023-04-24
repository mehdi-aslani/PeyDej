﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PeyDej.Models.Report;

[Table("RepairRequest",Schema = "Report")]
public partial class RepairRequest
{
    public long Id { get; set; }

    public DateTime InsDate { get; set; }

    [Display(Name = "عنوان")]
    public string? Caption { get; set; }

    [DisplayName("دپارتمان")]
    public long DepartmentId { get; set; }

    [Display(Name = "فرآیند")]
    public long? ProcessId { get; set; }

    [Display(Name = "تاریخ")]
    [NotMapped]
    public string DateDto { get; set; }

    [Display(Name = "تاریخ")]
    public DateTime? Date { get; set; }

    [Display(Name = "گزارش دهنده")]
    public string? Reporter { get; set; }

    [Display(Name = "نوع درخواست")]
    public long? RepairKindId { get; set; }

    [Display(Name = "ماشین")]
    public long? MachineId { get; set; }

    [Display(Name = "موتور")]
    public long? MotorId { get; set; }

    [Display(Name = "قطعه")]
    public string? SparePartId { get; set; }

    [Display(Name = "عنوان")]
    public int? Status { get; set; }
}