using System.Collections.Generic;

namespace backend.Modules.BusinessMemory.DTOs;

public class CopilotContextResponseDto
{
    public string RelevantContext { get; set; } = string.Empty;
    public object? CurrentData { get; set; }
    public IEnumerable<MemoryDto> RelevantMemories { get; set; } = new List<MemoryDto>();
    public IEnumerable<string> SuggestedSources { get; set; } = new List<string>();
}
