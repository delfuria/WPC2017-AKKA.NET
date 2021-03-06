﻿using Akka.Actor;
using Akka.Configuration;

namespace AKKA.Library.Demo
{
    public class SimpleActorSystem
    {
        private static Config _defaultConfig;
        private static Config _config;

        public static Config Config
        {
            get
            {
                return _config ?? _defaultConfig;
                //if (_config == null)
                //    return _defaultConfig;
                //return _config;
            }
            set
            {
                if (_config == null && value != null)
                    _config = value;
            }
        }

        private static string _name;

        public static string Name
        {
            get
            {
                if (_name.IsNullOrEmpty())
                    return "AKKA-NET";
                return _name;
            }
            set
            {
                if (_name.IsNullOrEmpty() && !value.IsNullOrEmpty())
                    _name = value;
            }
        }

        private static ActorSystem _actorSystem;

        public static ActorSystem Instance
        {
            get
            {
                if (_actorSystem == null)
                    _actorSystem = ActorSystem.Create(Name, Config);
                return _actorSystem;
            }
        }

        static SimpleActorSystem()
        {
            _defaultConfig = ConfigurationFactory.ParseString(@"
                    akka {
                    loglevel = DEBUG
                    loggers = [""AKKA.Library.Demo.TracerXLogger, AKKA.Library.Demo""]
                        actor
                        {
                            debug
                            {
                            //receive = on      # log any received message
                            autoreceive = on  # log automatically received messages, e.g. PoisonPill
                            lifecycle = on    # log actor lifecycle changes
                            event-stream = on # log subscription changes for Akka.NET event stream
                            unhandled = on    # log unhandled messages sent to actors
                            }
                        serializers {
                          hyperion = ""Akka.Serialization.HyperionSerializer, Akka.Serialization.Hyperion""
                        }
                            serialization-bindings {
                          ""System.Object"" = hyperion
                        }
                    }
                    }");
        }
    }
}