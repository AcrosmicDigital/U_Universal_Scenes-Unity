﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace U.Universal.Scenes
{
    public class SceneOperation
    {

        // True when the operation is set as successful or failed
        protected bool isDone;
        public bool IsDone { get => isDone; }

        // True when the operation is set to succesful and false when is set to failed
        protected bool isSuccessful;
        public bool IsSuccessful { get => isSuccessful; }

        // Contains the exeption when the operation fails, and is null when the operation is successful
        protected Exception error;
        public Exception Error { get => error; }

        // Contains a message when the operation is successful, and is null when the operation is failed
        protected string message;
        public string Message { get => message; }


        /// <summary>
        /// Constructor
        /// </summary>
        public SceneOperation()
        {
            this.isDone = false;
            this.isSuccessful = false;
            this.error = null;
            this.message = "";
        }


        /// <summary>
        /// Used to set the operation as failed when the operation is not done
        /// </summary>
        /// <param name="error">Error that cause the fail of the operation</param>
        /// <returns></returns>
        public SceneOperation Fails(Exception error)
        {
            if (isDone)
                throw new InvalidOperationException("SceneOperation is already set");

            this.isDone = true;
            this.error = error;
            return this;
        }


        /// <summary>
        /// Used to set the operation as succesfull when the operation is not done
        /// </summary>
        /// <param name="message">Optional message</param>
        /// <returns></returns>
        public SceneOperation Successful(string message = "")
        {
            if (isDone)
                throw new InvalidOperationException("SceneOperation is already set");

            this.isDone = true;
            this.isSuccessful = true;
            this.message = message;
            return this;
        }


        /// <summary>
        /// Implicit cast of operation to bool
        /// </summary>
        /// <param name="operation">The operation</param>
        public static implicit operator bool(SceneOperation operation)
        {
            return operation.IsSuccessful;
        }


        /// <summary>
        /// Implicit cast of operation to string
        /// </summary>
        /// <param name="operation">The operation</param>
        // Automatic conversion to string
        public static implicit operator string(SceneOperation operation)
        {
            if (!operation.isDone)
            {
                return "In progress";
            }
            else if (operation)
            {
                if (operation.Message == null)
                    return "Completed successfully";
                else
                    return "Completed successfully: " + operation.Message;
            }
            else
            {
                return "Failed: " + operation.Error;
            }
        }



        /// <summary>
        /// Implicit cast to int
        /// </summary>
        /// <param name="operation"></param>
        public static implicit operator int(SceneOperation operation)
        {
            // Try to parse the message to a int
            if (Int32.TryParse(operation.message, out int value))
                return value;
            else
                return 0;
        }


        /// <summary>
        /// Implicit cast to float
        /// </summary>
        /// <param name="operation"></param>
        public static implicit operator float(SceneOperation operation)
        {
            // Try to parse the message to a int
            if (float.TryParse(operation.message, out float value))
                return value;
            else
                return 0;
        }

    }


}
