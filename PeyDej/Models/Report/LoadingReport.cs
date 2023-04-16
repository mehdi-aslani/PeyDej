﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PeyDej.Models.Report;

[Table(name: "LoadingReport", Schema = "Report")]
public class LoadingReport
{
    [Key] public long Id { get; set; }

    public DateTime InsDate { get; set; } = DateTime.Now;

    [Display(Name = "تاریخ")]
    [Required(ErrorMessage = "مقدار {0} الزامی می باشد")]
    public long Date { get; set; }

    [Display(Name = "تاریخ")]
    [Required(ErrorMessage = "مقدار {0} الزامی می باشد")]
    public string DayCaption { get; set; } = null!;

    [Display(Name = "نوع تیغه")] public long? BladKindId { get; set; }

    /// <summary>
    /// SELECT * FROM  Base.SubCategory WHERE CategoryId = 8
    /// </summary>
    ///
    [Display(Name = "موجودی قابل باگیری کوره ها")]
    public long? LoadingIntervalId { get; set; }

    [Display(Name = "توضیحات")] public string? Description { get; set; }

    [Display(Name = "سایر تیغه ها")] public string? OtherBladCaption { get; set; }
}