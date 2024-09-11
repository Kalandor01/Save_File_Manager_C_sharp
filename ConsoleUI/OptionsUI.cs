﻿using ConsoleUI.Keybinds;
using ConsoleUI.UIElements;
using ConsoleUI.UIElements.EventArgs;
using System.Text;

namespace ConsoleUI
{
    /// <summary>
    /// Object for displaying an options UI using the <c>Display</c> function.<br/>
    /// Prints the <c>title</c> and then the list of elements that the user can cycle between with the selected keys (arrow keys by default) and interact with them.
    /// </summary>
    public class OptionsUI
    {
        #region Public fields
        /// <summary>
        /// The list of <c>BaseUI</c> objects to use.
        /// </summary>
        public IEnumerable<BaseUI?> elements;
        /// <summary>
        /// The string to print before the <c>elements</c>.
        /// </summary>
        public string? title;
        /// <summary>
        /// The cursor icon style to use.
        /// </summary>
        public CursorIcon cursorIcon;
        /// <summary>
        /// Allows the user to press the key associated with escape, to exit the menu.
        /// </summary>
        public bool canEscape;
        /// <summary>
        /// Whether to pass in the object into the element's functions.
        /// </summary>
        public bool passInObject;
        /// <summary>
        /// The settings for the scrolling of UI elements.
        /// </summary>
        public ScrollSettings scrollSettings;
        /// <summary>
        /// The index of the currently selected element in the list.
        /// </summary>
        public int selected;
        /// <summary>
        /// The start index of the currently displayed section of the elements list.
        /// </summary>
        public int startIndex;
        /// <summary>
        /// The text to display, to clear the screen.
        /// </summary>
        public string clearScreenText;
        #endregion

        #region Event delegates
        /// <summary>
        /// Called before the UI options are displayed.
        /// </summary>
        /// <param name="sender">The sender of this event.</param>
        /// <param name="args">The arguments for this event.</param>
        public delegate void BeforeOptionsDisplayedEventHandler(OptionsUI sender, BeforeOptionsDisplayedEventArgs args);

        /// <summary>
        /// Called after the UI options are displayed.
        /// </summary>
        /// <param name="sender">The sender of this event.</param>
        /// <param name="args">The arguments for this event.</param>
        public delegate void AfterOptionsDisplayedEventHandler(OptionsUI sender, AfterOptionsDisplayedEventArgs args);

        /// <summary>
        /// Called before a UI element text is created.
        /// </summary>
        /// <param name="sender">The sender of this event.</param>
        /// <param name="args">The arguments for this event.</param>
        public delegate void BeforeOptionsTextCreatedEventHandler(OptionsUI sender, BeforeOptionsTextCreatedEventArgs args);

        /// <summary>
        /// Called after a UI element text is created.
        /// </summary>
        /// <param name="sender">The sender of this event.</param>
        /// <param name="args">The arguments for this event.</param>
        public delegate void AfterOptionsTextCreatedEventHandler(OptionsUI sender, AfterOptionsTextCreatedEventArgs args);

        /// <summary>
        /// Called after a UI element text is displayed.
        /// </summary>
        /// <param name="sender">The sender of this event.</param>
        /// <param name="args">The arguments for this event.</param>
        public delegate void AfterOptionsTextDisplayedEventHandler(OptionsUI sender, AfterOptionsTextDisplayedEventArgs args);

        /// <summary>
        /// Called when a key is pressed.
        /// </summary>
        /// <param name="sender">The sender of this event.</param>
        /// <param name="args">The arguments for this event.</param>
        public delegate void OptionsKeyPressedEventHandler(OptionsUI sender, OptionsKeyPressedEventArgs args);
        #endregion

        #region Events
        /// <summary>
        /// Called before the UI options are displayed.
        /// </summary>
        public event BeforeOptionsDisplayedEventHandler BeforeOptionsDisplayed;

        /// <summary>
        /// Called after the UI options are displayed.
        /// </summary>
        public event AfterOptionsDisplayedEventHandler AfterOptionsDisplayed;

        /// <summary>
        /// Called before a UI element text is created.
        /// </summary>
        public event BeforeOptionsTextCreatedEventHandler BeforeTextCreated;

        /// <summary>
        /// Called after a UI element is text is created.
        /// </summary>
        public event AfterOptionsTextCreatedEventHandler AfterTextCreated;

        /// <summary>
        /// Called after a UI element text is displayed.
        /// </summary>
        public event AfterOptionsTextDisplayedEventHandler AfterTextDisplayed;

        /// <summary>
        /// Called when a key is pressed.<br/>
        /// Returns if the input handling should continue (and the menu should refresh).
        /// </summary>
        public event OptionsKeyPressedEventHandler KeyPressed;
        #endregion

        #region Public constructors
        /// <summary>
        /// <inheritdoc cref="OptionsUI"/>
        /// </summary>
        /// <param name="elements"><inheritdoc cref="elements" path="//summary"/></param>
        /// <param name="title"><inheritdoc cref="title" path="//summary"/></param>
        /// <param name="cursorIcon"><inheritdoc cref="cursorIcon" path="//summary"/></param>
        /// <param name="canEscape"><inheritdoc cref="canEscape" path="//summary"/></param>
        /// <param name="passInObject"><inheritdoc cref="passInObject" path="//summary"/></param>
        /// <param name="scrollSettings"><inheritdoc cref="scrollSettings" path="//summary"/></param>
        /// <param name="clearScreenText"><inheritdoc cref="clearScreenText" path="//summary"/><br/>
        /// By default, it's 70 newlines (faster than actualy clearing the screen).</param>
        /// <exception cref="UINoSelectablesExeption">Exceptions thrown, if there are no selectable UI elements in the list.</exception>
        public OptionsUI(
            IEnumerable<BaseUI?> elements,
            string? title = null,
            CursorIcon? cursorIcon = null,
            bool canEscape = true,
            bool passInObject = true,
            ScrollSettings? scrollSettings = null,
            string? clearScreenText = null)
        {
            if (elements.All(answer => answer is null || !answer.IsSelectable))
            {
                throw new UINoSelectablesExeption();
            }
            this.elements = elements;
            this.title = title;
            this.cursorIcon = cursorIcon ?? new CursorIcon();
            this.canEscape = canEscape;
            this.passInObject = passInObject;
            this.scrollSettings = scrollSettings ?? new ScrollSettings();
            this.clearScreenText = clearScreenText ?? "\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n";
        }
        #endregion

        #region EventCallFunctions
        /// <summary>
        /// Calls the <c>BeforeOptionsDisplayed</c> event.
        /// </summary>
        private void RaiseBeforeOptionsDisplayedEvent(BeforeOptionsDisplayedEventArgs args)
        {
            if (BeforeOptionsDisplayed is not null)
            {
                BeforeOptionsDisplayed(this, args);
            }
        }

        /// <summary>
        /// Calls the <c>AfterOptionsDisplayed</c> event.
        /// </summary>
        private void RaiseAfterOptionsDisplayedEvent(AfterOptionsDisplayedEventArgs args)
        {
            if (AfterOptionsDisplayed is not null)
            {
                AfterOptionsDisplayed(this, args);
            }
        }

        /// <summary>
        /// Calls the <c>BeforeTextCreated</c> event.
        /// </summary>
        private void RaiseBeforeTextCreatedEvent(BeforeOptionsTextCreatedEventArgs args)
        {
            if (BeforeTextCreated is not null)
            {
                BeforeTextCreated(this, args);
            }
        }

        /// <summary>
        /// Calls the <c>AfterTextCreated</c> event.
        /// </summary>
        protected void RaiseAfterTextCreatedEvent(AfterOptionsTextCreatedEventArgs args)
        {
            if (AfterTextCreated is not null)
            {
                AfterTextCreated(this, args);
            }
        }

        /// <summary>
        /// Calls the <c>AfterTextDisplayed</c> event.
        /// </summary>
        protected void RaiseAfterTextDisplayedEvent(AfterOptionsTextDisplayedEventArgs args)
        {
            if (AfterTextDisplayed is not null)
            {
                AfterTextDisplayed(this, args);
            }
        }

        /// <summary>
        /// Calls the <c>KeyPressed</c> event.
        /// </summary>
        protected void RaiseKeyPressedEvent(OptionsKeyPressedEventArgs args)
        {
            if (KeyPressed is not null)
            {
                KeyPressed(this, args);
            }
        }
        #endregion

        private void DisplayOptions()
        {
            var beforeDisplayArgs = new BeforeOptionsDisplayedEventArgs();
            RaiseBeforeOptionsDisplayedEvent(beforeDisplayArgs);
            if (beforeDisplayArgs.OverrideText != null)
            {
                Console.WriteLine(beforeDisplayArgs.OverrideText);
                return;
            }

            // clear screen + render
            var txtBeginning = new StringBuilder(clearScreenText);

            // title
            if (title is not null)
            {
                txtBeginning.Append($"{title}\n\n");
            }

            // elements
            int endIndex;
            if (scrollSettings.maxElements == -1 || scrollSettings.maxElements >= elements.Count())
            {
                startIndex = 0;
                endIndex = elements.Count();
            }
            else
            {
                if (startIndex > selected - scrollSettings.scrollUpMargin)
                {
                    startIndex = selected - scrollSettings.scrollUpMargin;
                }
                if (startIndex + scrollSettings.maxElements - 1 < selected + scrollSettings.scrollDownMargin)
                {
                    startIndex = selected + scrollSettings.scrollDownMargin - (scrollSettings.maxElements - 1);
                }

                startIndex = Math.Clamp(startIndex, 0, elements.Count() - 1);
                endIndex = Math.Clamp(startIndex + scrollSettings.maxElements, 0, elements.Count());
                startIndex = Math.Clamp(endIndex - scrollSettings.maxElements, 0, elements.Count() - 1);
            }

            txtBeginning.Append(startIndex == 0 ? scrollSettings.scrollIcon.topEndIndicator : scrollSettings.scrollIcon.topContinueIndicator);
            Console.Write(txtBeginning.ToString());
            for (var x = startIndex; x < endIndex; x++)
            {
                var element = elements.ElementAt(x)!;
                var beforeTextCreatedArgs = new BeforeOptionsTextCreatedEventArgs(x);
                RaiseBeforeTextCreatedEvent(beforeTextCreatedArgs);
                if (beforeTextCreatedArgs.OverrideText != null)
                {
                    Console.Write(beforeTextCreatedArgs.OverrideText);
                    continue;
                }

                var elementText = element?.MakeText(
                        selected == x ? cursorIcon.sIcon : cursorIcon.icon,
                        selected == x ? cursorIcon.sIconR : cursorIcon.iconR,
                        passInObject ? this : null
                    ) ?? "\n";

                var afterTextCreatedArgs = new AfterOptionsTextCreatedEventArgs(elementText, x);
                RaiseAfterTextCreatedEvent(afterTextCreatedArgs);

                Console.Write(afterTextCreatedArgs.OverrideText ?? elementText);

                var afterTextDisplayedArgs = new AfterOptionsTextDisplayedEventArgs(elementText, x);
                RaiseAfterTextDisplayedEvent(afterTextDisplayedArgs);
            }

            Console.WriteLine(endIndex == elements.Count() ? scrollSettings.scrollIcon.bottomEndIndicator : scrollSettings.scrollIcon.bottomContinueIndicator);

            var afterDisplayArgs = new AfterOptionsDisplayedEventArgs(endIndex);
            RaiseAfterOptionsDisplayedEvent(afterDisplayArgs);
        }

        /// <summary>
        /// Prints the title and then a list of elements that the user can cycle between with the up and down arrows, and adjust with either the left and right arrow keys or the enter pressedKey depending on the input object type, and exit with the pressedKey assigned to escape.<br/>
        /// if an element in the list is null, the line will be blank and cannot be selected.
        /// </summary>
        /// <param name="keybinds">The list of <c>KeyAction</c> objects to use. The order of the actions should be:<br/>
        /// - escape, up, down, left, right, enter.</param>
        /// <exception cref="UINoSelectablesExeption">Exceptions thrown, if there are no selectable UI elements in the list.</exception>
        public object? Display(IEnumerable<KeyAction>? keybinds = null)
        {
            // no selectable element
            if (elements.All(element => element is null || !element.IsSelectable))
            {
                throw new UINoSelectablesExeption();
            }

            // keybinds
            if (keybinds is null || keybinds.Count() < 6)
            {
                keybinds = Utils.GetDefaultKeybinds();
            }
            cursorIcon ??= new CursorIcon();

            // is enter needed?
            var enterKeyNeeded = elements.Any(element => element is not null && element.IsClickable);
            // put selected on selectable
            selected = 0;
            while (elements.ElementAt(selected)?.IsSelectable != true)
            {
                selected++;
            }

            startIndex = Math.Clamp(0, selected - scrollSettings.scrollUpMargin, elements.Count() - 1);

            // render/getkey loop
            KeyAction pressedKey;
            do
            {
                // prevent infinite loop
                if (elements.All(answer => answer is null || !answer.IsSelectable))
                {
                    throw new UINoSelectablesExeption();
                }

                DisplayOptions();

                // move selection/change value
                var actualMove = false;
                do
                {
                    // get pressedKey
                    pressedKey = keybinds.ElementAt((int)Key.ENTER);
                    var selectedElement = elements.ElementAt(selected);
                    if (
                        selectedElement is not null &&
                        selectedElement.IsClickable &&
                        selectedElement.IsOnlyClickable
                    )
                    {
                        pressedKey = Utils.GetKey(GetKeyMode.IGNORE_HORIZONTAL, keybinds);
                    }
                    else
                    {
                        while (pressedKey.Equals(keybinds.ElementAt((int)Key.ENTER)))
                        {
                            pressedKey = Utils.GetKey(GetKeyMode.NO_IGNORE, keybinds);
                            if (pressedKey.Equals(keybinds.ElementAt((int)Key.ENTER)) && !enterKeyNeeded)
                            {
                                pressedKey = keybinds.ElementAt((int)Key.ESCAPE);
                            }
                        }
                    }

                    var keyPressedEventArgs = new OptionsKeyPressedEventArgs(pressedKey, keybinds);
                    RaiseKeyPressedEvent(keyPressedEventArgs);
                    if (keyPressedEventArgs.UpdateScreen != null)
                    {
                        actualMove = (bool)keyPressedEventArgs.UpdateScreen;
                    }
                    if (keyPressedEventArgs.CancelKeyHandling)
                    {
                        continue;
                    }

                    // move selection
                    if (
                        pressedKey.Equals(keybinds.ElementAt((int)Key.UP)) ||
                        pressedKey.Equals(keybinds.ElementAt((int)Key.DOWN))
                    )
                    {
                        var prevSelected = selected;
                        while (true)
                        {
                            selected += pressedKey.Equals(keybinds.ElementAt((int)Key.DOWN)) ? 1 : -1;
                            selected %= elements.Count();
                            if (selected < 0)
                            {
                                selected = elements.Count() - 1;
                            }
                            var newSelected = elements.ElementAt(selected);
                            if (
                                newSelected is not null &&
                                newSelected.IsSelectable
                            )
                            {
                                break;
                            }
                        }
                        if (prevSelected != selected)
                        {
                            actualMove = true;
                        }
                    }
                    // change value
                    else if (
                        selectedElement is not null &&
                        selectedElement.IsSelectable &&
                        !pressedKey.Equals(keybinds.ElementAt((int)Key.ESCAPE))
                    )
                    {
                        var returned = selectedElement.HandleAction(pressedKey, keybinds, passInObject ? this : null);
                        if (returned is not null)
                        {
                            if (returned.GetType() == typeof(bool))
                            {
                                actualMove = (bool)returned;
                            }
                            else
                            {
                                return returned;
                            }
                        }
                    }
                    else if (canEscape && pressedKey.Equals(keybinds.ElementAt((int)Key.ESCAPE)))
                    {
                        actualMove = true;
                    }
                }
                while (!actualMove);
            }
            while (!canEscape || !pressedKey.Equals(keybinds.ElementAt((int)Key.ESCAPE)));
            return null;
        }
    }
}
