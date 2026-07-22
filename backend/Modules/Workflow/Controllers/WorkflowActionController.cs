using backend.Modules.Workflow.Constants;
using Microsoft.AspNetCore.Mvc;

namespace backend.Modules.Workflow.Controllers;

[Route("api/workflows/actions")]
public class WorkflowActionController : BaseWorkflowController
{
    [HttpGet("types")]
    public ActionResult<IEnumerable<string>> GetActionTypes()
    {
        var types = Enum.GetNames<ActionType>();
        return Ok(types);
    }
}
