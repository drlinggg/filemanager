using Commands.Common;
using FlagHandlers.Common;
using Flags.Common;
using FlagVisitors.Models;
using Receivers.Common;
using ResultTypes.CommandResults;

namespace Commands.Models;

public class FileMove : BaseCommand
{
    private readonly IFlagHandler? _handler = null;

    private readonly string _sourcePath;

    private readonly string _destPath;

    public override ExecuteResult Execute()
    {
        // if success return success else else
        Receiver.FileMove(_sourcePath, _destPath);
        return new ExecuteResult.Success();
    }

    public FileMove(IReceiver receiver, string sourcePath, string destPath) : base(receiver)
    {
        _sourcePath = sourcePath;
        _destPath = destPath;
    }

    public override IFlag? Handle(IEnumerator<string> request)
    {
        if (_handler is null)
            return null;

        IFlag? flag = _handler.Handle(request);

        if (flag != null)
        {
            // Используем посетителя для изменения состояния команды
            var visitor = new FlagModifierVisitor(this);
            flag.Accept(visitor);
        }

        return flag;
    }
}
