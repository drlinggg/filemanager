using Commands.Common;
using Flags.Common;
using ParamHandlers.Common;
using ParamHandlers.Models;
using Receivers.Models;

namespace Programs;

public class Program
{
    public static void Main(string[] args)
    {
        IParamHandler handler = new ConnectHandler()
            .AddNext(new DisconnectHandler())
            .AddNext(new FileCopyHandler())
            .AddNext(new FileMoveHandler())
            .AddNext(new FileShowHandler())
            .AddNext(new FileRemoveHandler())
            .AddNext(new FileRenameHandler())
            .AddNext(new TreeListHandler())
            .AddNext(new TreegotoHandler());

        var fileManager = new FileManager();

        IEnumerator<string> request = ((IEnumerable<string>)args).GetEnumerator();
        bool flagg = true;
        ICommand? command = null;
        while (true)
        {
            if (flagg)
            {
                if (request.MoveNext() == false)
                    break;
            }

            command = handler.Handle(request, fileManager);

            if (request.MoveNext() == false)
                break;

            flagg = true;
            if (command is null)
            {
                continue;
            }

            IFlag? flag = command.Handle(request);

            if (request.MoveNext() == false)
                break;

            while (flag is not null)
            {
                flag = command.Handle(request);
                if (flag is null)
                {
                    flagg = false;
                    break;
                }

                if (request.MoveNext() == false)
                    break;
            }

            command.Execute();
        }

        if (command is not null)
            command.Execute();

        while (true)
        {
            string? input = Console.ReadLine();
            if (input is null)
            {
                continue;
            }

            request = ((IEnumerable<string>)input.Split(' ')).GetEnumerator();

            if (request is null)
            {
                continue;
            }

            flagg = true;
            command = null;

            while (true)
            {
                if (flagg)
                {
                    if (request.MoveNext() == false)
                        break;
                }

                command = handler.Handle(request, fileManager);

                if (request.MoveNext() == false)
                    break;

                flagg = true;
                if (command is null)
                {
                    continue;
                }

                IFlag? flag = command.Handle(request);

                if (request.MoveNext() == false)
                    break;

                while (flag is not null)
                {
                    flag = command.Handle(request);
                    if (flag is null)
                    {
                        flagg = false;
                        break;
                    }

                    if (request.MoveNext() == false)
                        break;
                }

                command.Execute();
            }

            if (command is not null)
                command.Execute();
        }
    }
}
