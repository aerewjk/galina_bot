using galina_bot.Classes.Commands.Core;
namespace galina_bot.Classes.Commands
{
    internal class CommandOrchestrator
    {
        private CommandParser parser;
        private CommandProcessor processor;
        private CommandExecutor executor;

        public CommandOrchestrator()
        {
            parser = new CommandParser();
            processor = new CommandProcessor();
            executor = new CommandExecutor();
        }
        public bool CommandOrchestrate(string recognizedText)
        {
            string[] recognizedWords = parser.CommandParse(recognizedText);

            List<Action<string[]>>? selectedAction = processor.CommandProcess(recognizedWords);

            if (selectedAction != null)
            {
                foreach (var spelledCommand in selectedAction)
                {
                    executor.CommandExecute(spelledCommand, recognizedWords);
                }
                return true;
            } 
            else 
            { 
                return false;
            }
        }
    }
}
