﻿namespace ConsoleUI.UIElements.EventArgs
{
    /// <summary>
    /// Class for storing the arguments of the <c>BeforeTextCreated</c> event.
    /// </summary>
    public class BeforeElementTextCreatedEventArgs
    {
        /// <summary>
        /// The index of the current BaseUI element.
        /// </summary>
        public readonly int currentUIIndex;

        /// <summary>
        /// If not null, halts text creartion and returns this text instead.
        /// </summary>
        public string? OverrideText { get; set; }

        /// <summary>
        /// <inheritdoc cref="BeforeElementTextCreatedEventArgs"/>
        /// </summary>
        /// <param name="currentUIIndex"><inheritdoc cref="currentUIIndex" path="//summary"/></param>
        /// <param name="overrideText"><inheritdoc cref="OverrideText" path="//summary"/></param>
        public BeforeElementTextCreatedEventArgs(
            int currentUIIndex,
            string? overrideText = null
        )
        {
            this.currentUIIndex = currentUIIndex;
            OverrideText = overrideText;
        }
    }
}
