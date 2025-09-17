using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekat18.Command
{
    public class CommandManager
    {
        private readonly Stack<ICommand> undoStack = new Stack<ICommand>();
        private readonly Stack<ICommand> redoStack = new Stack<ICommand>();

        public void ExecuteCommand(ICommand command)
        {
            command.Execute();
            undoStack.Push(command);
            redoStack.Clear();
        }

        public void Undo()
        {
            if (undoStack.Count > 0)
            {
                var command = undoStack.Pop();
                command.Undo();
                redoStack.Push(command);
            }
        }

        public void Redo()
        {
            if (redoStack.Count > 0)
            {
                var command = redoStack.Pop();
                command.Execute();
                undoStack.Push(command);
            }
        }

        public bool CanUndo => undoStack.Count > 0;
        public bool CanRedo => redoStack.Count > 0;

        public void Clear()
        {
            undoStack.Clear();
            redoStack.Clear();
        }

        public CommandManager() { }

    }
}
