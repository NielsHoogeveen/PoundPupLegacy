﻿using System.ComponentModel.DataAnnotations;

namespace PoundPupLegacy.EditModel;

public interface Node
{
    int? NodeId { get; }
    int? UrlId { get; set; }
    int PublisherId { get; set; }
    int OwnerId { get; set; }
    string Title { get; set; }
    List<Tags> Tags { get; }
    List<Tenant> Tenants { get; }
    List<TenantNode> TenantNodes { get; }
    List<File> Files { get; }
    string NodeTypeName { get; }
}
