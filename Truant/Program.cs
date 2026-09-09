using Truant.Core;
using Truant.Strategies;

var strategy = new FirstSkipStrategy();
var runner = new MultipleSimulationRunner(strategy);

runner.Run();