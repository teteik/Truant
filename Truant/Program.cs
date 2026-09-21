using Truant.Core;
using Truant.Strategies;

var strategy = new FirstSkipStrategy();
//var strategy = new LectorsStrtegy();
var runner = new MultipleSimulationRunner(strategy);
//var askingCounter = new AskingCounter(new DefaultRandomProvider());

//askingCounter.RunProfessorsSimulation();

//Console.WriteLine();

runner.Run(); 