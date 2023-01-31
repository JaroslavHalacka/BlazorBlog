Scaffold commands - data first
------------------------------
Scaffold-DbContext -Connection Name=DefaultConnection Microsoft.EntityFrameworkCore.SqlServer -OutputDir "Models" -Namespace "BlazorBlog.Models" -Context "DataContext"  -ContextDir "Data" -ContextNamespace "BlazorBlog.Data" -force
