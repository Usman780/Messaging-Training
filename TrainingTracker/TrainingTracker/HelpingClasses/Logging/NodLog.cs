using NLog;
using NLog.Config;
using NLog.Targets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrainingTracker.HelpingClasses.Logging
{
    /// <summary>
    /// This class is internal class to maintain logs. All the logs should be handled by explicitly this class.
    /// </summary>
    public class NodLog
    {
        public Logger logger;
        /// <summary>
        /// Nodlog 
        /// </summary>
        /// <param name="controllerName"></param>
        public NodLog (string controllerName)
        {
            var config = new LoggingConfiguration();

            // Step 2. Create targets



            var fileTarget = new FileTarget("target2")
            {
                FileName = "logs/" + controllerName + ".txt",
                Layout = "${longdate} ${level} ${message}  ${exception}"
            };
            config.AddTarget(fileTarget);


          

            // Step 3. Define rules
            // only errors to file
            config.AddRuleForAllLevels(fileTarget); // all to console

            // Step 4. Activate the configuration
            LogManager.Configuration = config;
            logger = LogManager.GetLogger("logger");

        }
    }
}