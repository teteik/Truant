using Truant;
using Truant.Core;
using Truant.Strategies;
using Truant.Utils;

var strategy = new FirstSkipStrategy();
var runner = new MultipleSimulationRunner(strategy);
var askingCounter = new AskingCounter(new DefaultRandomProvider());

askingCounter.RunProfessorsSimulation();

Console.WriteLine();

runner.Run(); 