using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_Document.Models;

public partial class DocumentDetail
{
    public string DocNo { get; set; } = null!;

    public DateTime? CreateDate { get; set; }

    public string ProjectName { get; set; } = null!;


    [Key]
    public string ProjectCode { get; set; } = null!;

    public DateTime? StartDate { get; set; }

    public DateTime? Projectdate { get; set; }

    public DateTime? EndDate { get; set; }

    public string Location { get; set; } = null!;

    public int? Maintenancemoney { get; set; }

    public string? Maintenancemoneytext { get; set; }

    public int? Accumulatedmoney { get; set; }

    public string? Accumulatedmoneytext { get; set; }

    public int? Othermoney { get; set; }

    public string? Othermoneytext { get; set; }

    public int? _1universityactivitiesSt { get; set; }

    public int? _1universityactivities { get; set; }

    public int? _211publicspiritactivitiesSt { get; set; }

    public int? _211publicspiritactivities { get; set; }

    public int? _212moralSt { get; set; }

    public int? _212moral { get; set; }

    public int? _221competencybuildingactivitiesSt { get; set; }

    public int? _221competencybuildingactivities { get; set; }

    public int? _222itskillsSt { get; set; }

    public int? _222itskills { get; set; }

    public int? _223developingSt { get; set; }

    public int? _223developing { get; set; }

    public int? _231democraticSt { get; set; }

    public int? _231democratic { get; set; }

    public int? _232relationshipsSt { get; set; }

    public int? _232relationships { get; set; }

    public int? _24healthSt { get; set; }

    public int? _24health { get; set; }

    public int? _31activitiesSt { get; set; }

    public int? _31activities { get; set; }

    public int? _32socialSt { get; set; }

    public int? _32social { get; set; }

    [NotMapped]
    public string DocNoDis { get; set; } = null!;

    [NotMapped]
    public DateTime? CreateDateDis { get; set; }
}
