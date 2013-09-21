//
// In order to convert some functionality to Visual C#, the Java Language Conversion Assistant
// creates "support classes" that duplicate the original functionality.  
//
// Support classes replicate the functionality of the original code, but in some cases they are 
// substantially different architecturally. Although every effort is made to preserve the 
// original architecture of the application in the converted project, the user should be aware that 
// the primary goal of these support classes is to replicate functionality, and that at times 
// the architecture of the resulting solution may differ somewhat.
//

using System;

	/// <summary>
	/// This interface should be implemented by any class whose instances are intended 
	/// to be executed by a thread.
	/// </summary>
	public interface IThreadRunnable
	{
		/// <summary>
		/// This method has to be implemented in order that starting of the thread causes the object's 
		/// run method to be called in that separately executing thread.
		/// </summary>
		void Run();
	}

/// <summary>
/// Contains conversion support elements such as classes, interfaces and static methods.
/// </summary>
public class SupportClass
{
	/// <summary>
	/// Recieves a form and an integer value representing the operation to perform when the closing 
	/// event is fired.
	/// </summary>
	/// <param name="form">The form that fire the event.</param>
	/// <param name="operation">The operation to do while the form is closing.</param>
	public static void CloseOperation(System.Windows.Forms.Form form, int operation)
	{
		switch (operation)
		{
			case 0:
				break;
			case 1:
				form.Hide();
				break;
			case 2:
				form.Dispose();
				break;
			case 3:
				form.Dispose();
				System.Windows.Forms.Application.Exit();
				break;
		}
	}


	/*******************************/
	/// <summary>
	/// Class used to store and retrieve an object command specified as a String.
	/// </summary>
	public class CommandManager
	{
		/// <summary>
		/// Private Hashtable used to store objects and their commands.
		/// </summary>
		private static System.Collections.Hashtable Commands = new System.Collections.Hashtable();

		/// <summary>
		/// Sets a command to the specified object.
		/// </summary>
		/// <param name="obj">The object that has the command.</param>
		/// <param name="cmd">The command for the object.</param>
		public static void SetCommand(System.Object obj, System.String cmd)
		{
			if (obj != null)
			{
				if (Commands.Contains(obj))
					Commands[obj] = cmd;
				else
					Commands.Add(obj, cmd);
			}
		}

		/// <summary>
		/// Gets a command associated with an object.
		/// </summary>
		/// <param name="obj">The object whose command is going to be retrieved.</param>
		/// <returns>The command of the specified object.</returns>
		public static System.String GetCommand(System.Object obj)
		{
			System.String result = "";
			if (obj != null)
				result = System.Convert.ToString(Commands[obj]);
			return result;
		}



		/// <summary>
		/// Checks if the Control contains a command, if it does not it sets the default
		/// </summary>
		/// <param name="button">The control whose command will be checked</param>
		public static void CheckCommand(System.Windows.Forms.ButtonBase button)
		{
			if (button != null)
			{
				if (GetCommand(button).Equals(""))
					SetCommand(button, button.Text);
			}
		}

		/// <summary>
		/// Checks if the Control contains a command, if it does not it sets the default
		/// </summary>
		/// <param name="button">The control whose command will be checked</param>
		public static void CheckCommand(System.Windows.Forms.MenuItem menuItem)
		{
			if (menuItem != null)
			{
				if (GetCommand(menuItem).Equals(""))
					SetCommand(menuItem, menuItem.Text);
			}
		}

		/// <summary>
		/// Checks if the Control contains a command, if it does not it sets the default
		/// </summary>
		/// <param name="button">The control whose command will be checked</param>
		public static void CheckCommand(System.Windows.Forms.ComboBox comboBox)
		{
			if (comboBox != null)
			{
				if (GetCommand(comboBox).Equals(""))
					SetCommand(comboBox,"comboBoxChanged");
			}
		}

	}
	/*******************************/
	/// <summary>
	/// This class contains static methods to manage TreeViews.
	/// </summary>
	public class TreeSupport
	{
		/// <summary>
		/// Creates a new TreeView from the provided HashTable.
		/// </summary> 
		/// <param name="hashTable">HashTable</param>		
		/// <returns>Returns the created tree</returns>
		public static System.Windows.Forms.TreeView CreateTreeView(System.Collections.Hashtable hashTable)
		{
			System.Windows.Forms.TreeView tree = new System.Windows.Forms.TreeView();
			return SetTreeView(tree,hashTable);
		}

		/// <summary>
		/// Sets a TreeView with data from the provided HashTable.
		/// </summary> 
		/// <param name="treeView">Tree.</param>
		/// <param name="hashTable">HashTable.</param>
		/// <returns>Returns the set tree.</returns>		
		public static System.Windows.Forms.TreeView SetTreeView(System.Windows.Forms.TreeView treeView, System.Collections.Hashtable hashTable)
		{
			foreach (System.Collections.DictionaryEntry myEntry in hashTable)
			{				
				System.Windows.Forms.TreeNode root = new System.Windows.Forms.TreeNode();

				if (myEntry.Value is System.Collections.ArrayList)
				{
					root.Text = "[";
					FillNode(root, (System.Collections.ArrayList)myEntry.Value);	
					root.Text = root.Text + "]";				
				}
				else if (myEntry.Value is System.Object[])
				{
					root.Text = "[";
					FillNode(root,(System.Object[])myEntry.Value);
					root.Text = root.Text + "]";
				}
				else if (myEntry.Value is System.Collections.Hashtable)
				{
					root.Text = "[";
					FillNode(root,(System.Collections.Hashtable)myEntry.Value);
					root.Text = root.Text + "]";
				}
				else
					root.Text = myEntry.ToString();

				treeView.Nodes.Add(root);					
			}
			return treeView;
		}
		

		/// <summary>
		/// Creates a new TreeView from the provided ArrayList.
		/// </summary> 
		/// <param name="arrayList">ArrayList.</param>		
		/// <returns>Returns the created tree.</returns>
		public static System.Windows.Forms.TreeView CreateTreeView(System.Collections.ArrayList arrayList)
		{
			System.Windows.Forms.TreeView tree = new System.Windows.Forms.TreeView();
			return SetTreeView(tree, arrayList);
		}

		/// <summary>
		/// Sets a TreeView with data from the provided ArrayList.
		/// </summary> 
		/// <param name="treeView">Tree.</param>
		/// <param name="arrayList">ArrayList.</param>
		/// <returns>Returns the set tree.</returns>
		public static System.Windows.Forms.TreeView SetTreeView(System.Windows.Forms.TreeView treeView, System.Collections.ArrayList arrayList)		
		{
			foreach (System.Object arrayEntry in arrayList)
			{
				System.Windows.Forms.TreeNode root = new System.Windows.Forms.TreeNode();
		
				if (arrayEntry is System.Collections.ArrayList)
				{
					root.Text = "[";
					FillNode(root, (System.Collections.ArrayList)arrayEntry);
					root.Text = root.Text + "]";
				}
				else if (arrayEntry is System.Collections.Hashtable)
				{
					root.Text = "[";
					FillNode(root,(System.Collections.Hashtable)arrayEntry);
					root.Text = root.Text + "]";
				}
				else if (arrayEntry is System.Object[])	
				{
					root.Text = "[";
					FillNode(root,(System.Object[])arrayEntry);
					root.Text = root.Text + "]";
				}
				else
					root.Text = arrayEntry.ToString();
				
		
				treeView.Nodes.Add(root);					
			}
			return treeView;
		}		
		
		/// <summary>
		/// Creates a new TreeView from the provided Object Array.
		/// </summary> 
		/// <param name="objectArray">Object Array.</param>		
		/// <returns>Returns the created tree.</returns>
		public static System.Windows.Forms.TreeView CreateTreeView(System.Object[] objectArray)
		{
			System.Windows.Forms.TreeView tree = new System.Windows.Forms.TreeView();
			return SetTreeView(tree, objectArray);
		}

		/// <summary>
		/// Sets a TreeView with data from the provided Object Array.
		/// </summary> 
		/// <param name="treeView">Tree.</param>
		/// <param name="objectArray">Object Array.</param>
		/// <returns>Returns the created tree.</returns>
		public static System.Windows.Forms.TreeView SetTreeView(System.Windows.Forms.TreeView treeView, System.Object[] objectArray)
		{
			foreach (System.Object arrayEntry in objectArray)
			{		
				System.Windows.Forms.TreeNode root = new System.Windows.Forms.TreeNode();

				if (arrayEntry is System.Collections.ArrayList)
				{
					root.Text = "[";
					FillNode(root,(System.Collections.ArrayList)arrayEntry);
					root.Text = root.Text + "]";
				}
				else if (arrayEntry is System.Collections.Hashtable)				
				{
					root.Text = "[";
					FillNode(root,(System.Collections.Hashtable)arrayEntry);						
					root.Text = root.Text + "]";		
				}
				else if (arrayEntry is System.Object[])
				{
					root.Text = "[";
					FillNode(root,(System.Object[])arrayEntry);					
					root.Text = root.Text + "]";
				}
				else
					root.Text = arrayEntry.ToString();

				treeView.Nodes.Add(root);
			}		
			return treeView;
		}		

		/// <summary>
		/// Creates a new TreeView with the provided TreeNode as root.
		/// </summary> 
		/// <param name="root">Root.</param>		
		/// <returns>Returns the created tree.</returns>
		public static System.Windows.Forms.TreeView CreateTreeView(System.Windows.Forms.TreeNode root)
		{
			System.Windows.Forms.TreeView tree = new System.Windows.Forms.TreeView();
			return SetTreeView(tree, root);
		}

		/// <summary>
		/// Sets a TreeView with the provided TreeNode as root.
		/// </summary>
		/// <param name="treeView">Tree</param>
		/// <param name="root">Root</param>
		/// <returns>Returns the created tree</returns>
		public static System.Windows.Forms.TreeView SetTreeView(System.Windows.Forms.TreeView treeView, System.Windows.Forms.TreeNode root)
		{
			if (root != null)
				treeView.Nodes.Add(root);
			return treeView;
		}
			
		/// <summary>
		/// Sets a TreeView with the provided TreeNode as root.
		/// </summary> 
		/// <param name="model">Root data model.</param>
		public static void SetModel(System.Windows.Forms.TreeView tree, System.Windows.Forms.TreeNode model)
		{
			tree.Nodes.Clear();
			tree.Nodes.Add(model);
		}
			
		/// <summary>
		/// Get the root TreeNode from a TreeView.
		/// </summary> 
		/// <param name="tree">Tree.</param>
		public static System.Windows.Forms.TreeNode GetModel(System.Windows.Forms.TreeView tree)
		{
			if (tree.Nodes.Count > 0 )
				return tree.Nodes[0];
			else
				return null;
		}

		/// <summary>
		/// Sets a TreeNode with data from the provided ArrayList instance.
		/// </summary> 
		/// <param name="treeNode">Node.</param>
		/// <param name="arrayList">ArrayList.</param>
		/// <returns>Returns the set node.</returns>
		public static System.Windows.Forms.TreeNode FillNode(System.Windows.Forms.TreeNode treeNode, System.Collections.ArrayList arrayList)
		{		
			foreach (System.Object arrayEntry in arrayList)
			{
				System.Windows.Forms.TreeNode root = new System.Windows.Forms.TreeNode();				

				if (arrayEntry is System.Collections.ArrayList)
				{
					root.Text = "[";
					FillNode(root, (System.Collections.ArrayList)arrayEntry);
					root.Text = root.Text + "]";
					treeNode.Nodes.Add(root);
					treeNode.Text = treeNode.Text + ", " + root.Text;
				}
				else if (arrayEntry is System.Object[])
				{					
					root.Text = "[";
					FillNode(root,(System.Object[])arrayEntry);	
					root.Text = root.Text + "]";
					treeNode.Nodes.Add(root);	
					treeNode.Text = treeNode.Text + ", " + root.Text;
				}
				else if (arrayEntry is System.Collections.Hashtable)
				{
					root.Text = "[";
					FillNode(root,(System.Collections.Hashtable)arrayEntry);	
					root.Text = root.Text + "]";
					treeNode.Nodes.Add(root);	
					treeNode.Text = treeNode.Text + ", " + root.Text;
				}
				else
				{
					treeNode.Nodes.Add(arrayEntry.ToString());
					if (!(treeNode.Text.Equals("")))
					{
						if (treeNode.Text[treeNode.Text.Length-1].Equals('['))
							treeNode.Text = treeNode.Text + arrayEntry.ToString();
						else
							treeNode.Text = treeNode.Text + ", " + arrayEntry.ToString();
					}
					else
						treeNode.Text = treeNode.Text + ", " + arrayEntry.ToString();
				}
			}
			return treeNode;
		}
		

		/// <summary>
		/// Sets a TreeNode with data from the provided ArrayList.
		/// </summary> 
		/// <param name="treeNode">Node.</param>
		/// <param name="objectArray">Object Array.</param>
		/// <returns>Returns the set node.</returns>
		
		public static System.Windows.Forms.TreeNode FillNode(System.Windows.Forms.TreeNode treeNode, System.Object[] objectArray)
		{
			foreach (System.Object arrayEntry in objectArray)
			{
				System.Windows.Forms.TreeNode root = new System.Windows.Forms.TreeNode();

				if (arrayEntry is System.Collections.ArrayList)
				{
					root.Text = "[";
					FillNode(root,(System.Collections.ArrayList)arrayEntry);									
					root.Text = root.Text + "]";
				}
				else if (arrayEntry is System.Collections.Hashtable)				
				{
					root.Text = "[";
					FillNode(root,(System.Collections.Hashtable)arrayEntry);
					root.Text = root.Text + "]";				
				}
				else
				{
					root.Nodes.Add(objectArray.ToString());
					root.Text = root.Text + ", " + objectArray.ToString();
				}
				treeNode.Nodes.Add(root);
				treeNode.Text = treeNode.Text + root.Text;
			}
			return treeNode;
		}
		
		/// <summary>		
		/// Sets a TreeNode with data from the provided Hashtable.		
		/// </summary> 		
		/// <param name="treeNode">Node.</param>		
		/// <param name="hashTable">Hash Table Object.</param>		
		/// <returns>Returns the set node.</returns>		
		public static System.Windows.Forms.TreeNode FillNode(System.Windows.Forms.TreeNode treeNode, System.Collections.Hashtable hashTable)
		{		
			foreach (System.Collections.DictionaryEntry myEntry in hashTable)
			{
				System.Windows.Forms.TreeNode root = new System.Windows.Forms.TreeNode();				

				if (myEntry.Value is System.Collections.ArrayList)
				{
					FillNode(root, (System.Collections.ArrayList)myEntry.Value);
					treeNode.Nodes.Add(root);
				}
				else if (myEntry.Value is System.Object[])
				{
					FillNode(root,(System.Object[])myEntry.Value);	
					treeNode.Nodes.Add(root);	
				}
				else
					treeNode.Nodes.Add(myEntry.Key.ToString());
			}
			return treeNode;
		}
	}
	/*******************************/
	/// <summary>
	/// Writes the exception stack trace to the received stream
	/// </summary>
	/// <param name="throwable">Exception to obtain information from</param>
	/// <param name="stream">Output sream used to write to</param>
	public static void WriteStackTrace(System.Exception throwable, System.IO.TextWriter stream)
	{
		stream.Write(throwable.StackTrace);
		stream.Flush();
	}

	/*******************************/
	/// <summary>
	/// This method works as a handler for the Control.Layout event, it arranges the controls into a container
	/// control in a rectangular grid (rows and columns).
	/// The size and location of each inner control will be calculated according the number of them in the 
	/// container.
	/// The number of columns, rows, horizontal and vertical spacing between the inner controls will are
	/// specified as array of object values in the Tag property of the container.
	/// If the number of rows and columns specified is not enought to allocate all the controls, the number of 
	/// columns will be increased in order to maintain the number of rows specified.
	/// </summary>
	/// <param name="event_sender">The container control in which the controls will be relocated.</param>
	/// <param name="eventArgs">Data of the event.</param>
	public static void GridLayoutResize(System.Object event_sender, System.Windows.Forms.LayoutEventArgs eventArgs)
	{
		System.Windows.Forms.Control container = (System.Windows.Forms.Control) event_sender;
		if ((container.Tag is System.Drawing.Rectangle) && (container.Controls.Count > 0))
		{
			System.Drawing.Rectangle tempRectangle = (System.Drawing.Rectangle) container.Tag;

			if ((tempRectangle.X <= 0) && (tempRectangle.Y <= 0))
				throw new System.Exception("Illegal column and row layout count specified");

			int horizontal = tempRectangle.Width;
			int vertical = tempRectangle.Height;
			int count = container.Controls.Count;

			int rows = (tempRectangle.X == 0) ? (int) System.Math.Ceiling((double) (count / tempRectangle.Y)) : tempRectangle.X;
			int columns = (tempRectangle.Y == 0) ? (int) System.Math.Ceiling((double) (count / tempRectangle.X)) : tempRectangle.Y;
			
			rows = (rows == 0) ? 1 : rows;
			columns = (columns == 0) ? 1 : columns;

			while (count > rows * columns) columns++;

			int width = (container.DisplayRectangle.Width - horizontal * (columns - 1)) / columns;
			int height = (container.DisplayRectangle.Height - vertical * (rows - 1)) / rows;
			
			int indexColumn = 0;
			int indexRow = 0;

			foreach (System.Windows.Forms.Control tempControl in container.Controls)
			{
				int xCoordinate = indexColumn++ * (width + horizontal);
				int yCoordinate = indexRow * (height + vertical);
				tempControl.Location = new System.Drawing.Point(xCoordinate, yCoordinate);
				tempControl.Width = width;
				tempControl.Height = height;
				if (indexColumn == columns)
				{
					indexColumn = 0;
					indexRow++;
				}
			}
		}
	}


	/*******************************/
/// <summary>
/// Contains methods to construct customized Buttons
/// </summary>
public class ButtonSupport
{
	/// <summary>
	/// Creates a popup style Button with an specific text.	
	/// </summary>
	/// <param name="label">The text associated with the Button</param>
	/// <returns>The new Button</returns>
	public static System.Windows.Forms.Button CreateButton(System.String label)
	{			
		System.Windows.Forms.Button tempButton = new System.Windows.Forms.Button();
		tempButton.Text = label;
		tempButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		return tempButton;
	}

	/// <summary>
	/// Sets the an specific text for the Button
	/// </summary>
	/// <param name="Button">The button to be set</param>
	/// <param name="label">The text associated with the Button</param>
	public static void SetButton(System.Windows.Forms.ButtonBase Button, System.String label)
	{
		Button.Text = label;
		Button.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
	}

	/// <summary>
	/// Creates a Button with an specific text and style.
	/// </summary>
	/// <param name="label">The text associated with the Button</param>
	/// <param name="style">The style of the Button</param>
	/// <returns>The new Button</returns>
	public static System.Windows.Forms.Button CreateButton(System.String label, int style)
	{
		System.Windows.Forms.Button tempButton = new System.Windows.Forms.Button();
		tempButton.Text = label;
		setStyle(tempButton,style);
		return tempButton;
	}

	/// <summary>
	/// Sets the specific Text and Style for the Button
	/// </summary>
	/// <param name="Button">The button to be set</param>
	/// <param name="label">The text associated with the Button</param>
	/// <param name="style">The style of the Button</param>
	public static void SetButton(System.Windows.Forms.ButtonBase Button, System.String label, int style)
	{
		Button.Text = label;
		setStyle(Button,style);
	}

	/// <summary>
	/// Creates a standard style Button that contains an specific text and/or image
	/// </summary>
	/// <param name="control">The control to be contained analized to get the text and/or image for the Button</param>
	/// <returns>The new Button</returns>
	public static System.Windows.Forms.Button CreateButton(System.Windows.Forms.Control control)
	{
		System.Windows.Forms.Button tempButton = new System.Windows.Forms.Button();
		if(control.GetType().FullName == "System.Windows.Forms.Label")
		{
			tempButton.Image = ((System.Windows.Forms.Label)control).Image;
			tempButton.Text = ((System.Windows.Forms.Label)control).Text;
			tempButton.ImageAlign = ((System.Windows.Forms.Label)control).ImageAlign;
			tempButton.TextAlign = ((System.Windows.Forms.Label)control).TextAlign;
		}
		else
		{
			if(control.GetType().FullName == "System.Windows.Forms.PictureBox")//Tentative to see maps of UIGraphic
			{
				tempButton.Image = ((System.Windows.Forms.PictureBox)control).Image;
				tempButton.ImageAlign = ((System.Windows.Forms.Label)control).ImageAlign;
			}else
				tempButton.Text = control.Text;
		}
		tempButton.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
		return tempButton;
	}

	/// <summary>
	/// Sets an specific text and/or image to the Button
	/// </summary>
	/// <param name="Button">The button to be set</param>
	/// <param name="control">The control to be contained analized to get the text and/or image for the Button</param>
	public static void SetButton(System.Windows.Forms.ButtonBase Button,System.Windows.Forms.Control control)
	{
		if(control.GetType().FullName == "System.Windows.Forms.Label")
		{
			Button.Image = ((System.Windows.Forms.Label)control).Image;
			Button.Text = ((System.Windows.Forms.Label)control).Text;
			Button.ImageAlign = ((System.Windows.Forms.Label)control).ImageAlign;
			Button.TextAlign = ((System.Windows.Forms.Label)control).TextAlign;
		}
		else
		{
			if(control.GetType().FullName == "System.Windows.Forms.PictureBox")//Tentative to see maps of UIGraphic
			{
				Button.Image = ((System.Windows.Forms.PictureBox)control).Image;
				Button.ImageAlign = ((System.Windows.Forms.Label)control).ImageAlign;
			}
			else
				Button.Text = control.Text;
		}
		Button.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
	}

	/// <summary>
	/// Creates a Button with an specific control and style
	/// </summary>
	/// <param name="control">The control to be contained by the button</param>
	/// <param name="style">The style of the button</param>
	/// <returns>The new Button</returns>
	public static System.Windows.Forms.Button CreateButton(System.Windows.Forms.Control control, int style)
	{
		System.Windows.Forms.Button tempButton = CreateButton(control);
		setStyle(tempButton,style);
		return tempButton;
	}

	/// <summary>
	/// Sets an specific text and/or image to the Button
	/// </summary>
	/// <param name="Button">The button to be set</param>
	/// <param name="control">The control to be contained by the button</param>
	/// <param name="style">The style of the button</param>
	public static void SetButton(System.Windows.Forms.ButtonBase Button,System.Windows.Forms.Control control,int style)
	{
		SetButton(Button,control);
		setStyle(Button,style);
	}

	/// <summary>
	/// Sets the style of the Button
	/// </summary>
	/// <param name="Button">The Button that will change its style</param>
	/// <param name="style">The new style of the Button</param>
	/// <remarks> 
	/// If style is 0 then sets a popup style to the Button, otherwise sets a standard style to the Button.
	/// </remarks>
	public static void setStyle(System.Windows.Forms.ButtonBase Button, int style)
	{
		if (  (style == 0 ) || (style ==  67108864) || (style ==  33554432) ) 
			Button.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		else if ( (style == 2097152) || (style == 1048576) ||  (style == 16777216 ) )
				Button.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
		else 
			throw new System.ArgumentException("illegal style: " + style);		
	}

	/// <summary>
	/// Selects the Button
	/// </summary>
	/// <param name="Button">The Button that will change its style</param>
	/// <param name="select">It determines if the button woll be selected</param>
	/// <remarks> 
	/// If select is true thebutton will be selected, otherwise not.
	/// </remarks>
	public static void setSelected(System.Windows.Forms.ButtonBase Button, bool select)
	{
		if (select)
			Button.Select();
	}

	/// <summary>
	/// Receives a Button instance and sets the Text and Image properties.
	/// </summary>
	/// <param name="buttonInstance">Button instance to be set.</param>
	/// <param name="buttonText">Value to be set to Text.</param>
	/// <param name="icon">Value to be set to Image.</param>
	public static void SetStandardButton (System.Windows.Forms.ButtonBase buttonInstance, System.String buttonText , System.Drawing.Image icon )
	{
		buttonInstance.Text = buttonText;
		buttonInstance.Image = icon;
	}

	/// <summary>
	/// Creates a Button with a given text.
	/// </summary>
	/// <param name="buttonText">The text to be displayed in the button.</param>
	/// <returns>The new created button with text</returns>
	public static System.Windows.Forms.Button CreateStandardButton (System.String buttonText)
	{
		System.Windows.Forms.Button newButton = new System.Windows.Forms.Button();
		newButton.Text = buttonText;
		return newButton;
	}

	/// <summary>
	/// Creates a Button with a given image.
	/// </summary>
	/// <param name="buttonImage">The image to be displayed in the button.</param>
	/// <returns>The new created button with an image</returns>
	public static System.Windows.Forms.Button CreateStandardButton (System.Drawing.Image buttonImage)
	{
		System.Windows.Forms.Button newButton = new System.Windows.Forms.Button();
		newButton.Image = buttonImage;
		return newButton;
	}

	/// <summary>
	/// Creates a Button with a given image and a text.
	/// </summary>
	/// <param name="buttonText">The text to be displayed in the button.</param>
	/// <param name="buttonImage">The image to be displayed in the button.</param>
	/// <returns>The new created button with text and image</returns>
	public static System.Windows.Forms.Button CreateStandardButton (System.String buttonText, System.Drawing.Image buttonImage)
	{
		System.Windows.Forms.Button newButton = new System.Windows.Forms.Button();
		newButton.Text = buttonText;
		newButton.Image = buttonImage;
		return newButton;
	}
}
	/*******************************/
	/// <summary>
	/// Support Methods for FileDialog class. Note that several methods receive a DirectoryInfo object, but it won't be used in all cases.
	/// </summary>
	public class FileDialogSupport
	{
		/// <summary>
		/// Creates an OpenFileDialog open in a given path.
		/// </summary>
		/// <param name="path">Path to be opened by the OpenFileDialog.</param>
		/// <returns>A new instance of OpenFileDialog.</returns>
		public static System.Windows.Forms.OpenFileDialog CreateOpenFileDialog(System.IO.FileInfo path)
		{
			System.Windows.Forms.OpenFileDialog temp_fileDialog = new System.Windows.Forms.OpenFileDialog();
			temp_fileDialog.InitialDirectory = path.Directory.FullName;
			return temp_fileDialog;
		}

		/// <summary>
		/// Creates an OpenFileDialog open in a given path.
		/// </summary>
		/// <param name="path">Path to be opened by the OpenFileDialog.</param>
		/// <param name="directory">Directory to get the path from.</param>
		/// <returns>A new instance of OpenFileDialog.</returns>
		public static System.Windows.Forms.OpenFileDialog CreateOpenFileDialog(System.IO.FileInfo path, System.IO.DirectoryInfo directory)
		{
			System.Windows.Forms.OpenFileDialog temp_fileDialog = new System.Windows.Forms.OpenFileDialog();
			temp_fileDialog.InitialDirectory = path.Directory.FullName;
			return temp_fileDialog;
		}

		/// <summary>
		/// Creates a OpenFileDialog open in a given path.
		/// </summary>		
		/// <returns>A new instance of OpenFileDialog.</returns>
		public static System.Windows.Forms.OpenFileDialog CreateOpenFileDialog()
		{
			System.Windows.Forms.OpenFileDialog temp_fileDialog = new System.Windows.Forms.OpenFileDialog();
			temp_fileDialog.InitialDirectory = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);			
			return temp_fileDialog;
		}

		/// <summary>
		/// Creates an OpenFileDialog open in a given path.
		/// </summary>
		/// <param name="path">Path to be opened by the OpenFileDialog</param>
		/// <returns>A new instance of OpenFileDialog.</returns>
		public static System.Windows.Forms.OpenFileDialog CreateOpenFileDialog (System.String path)
		{
			System.Windows.Forms.OpenFileDialog temp_fileDialog = new System.Windows.Forms.OpenFileDialog();
			temp_fileDialog.InitialDirectory = path;
			return temp_fileDialog;
		}

		/// <summary>
		/// Creates an OpenFileDialog open in a given path.
		/// </summary>
		/// <param name="path">Path to be opened by the OpenFileDialog.</param>
		/// <param name="directory">Directory to get the path from.</param>
		/// <returns>A new instance of OpenFileDialog.</returns>
		public static System.Windows.Forms.OpenFileDialog CreateOpenFileDialog(System.String path, System.IO.DirectoryInfo directory)
		{
			System.Windows.Forms.OpenFileDialog temp_fileDialog = new System.Windows.Forms.OpenFileDialog();
			temp_fileDialog.InitialDirectory = path;
			return temp_fileDialog;
		}

		/// <summary>
		/// Modifies an instance of OpenFileDialog, to open a given directory.
		/// </summary>
		/// <param name="fileDialog">OpenFileDialog instance to be modified.</param>
		/// <param name="path">Path to be opened by the OpenFileDialog.</param>
		/// <param name="directory">Directory to get the path from.</param>
		public static void SetOpenFileDialog(System.Windows.Forms.FileDialog fileDialog, System.String path, System.IO.DirectoryInfo directory)
		{
			fileDialog.InitialDirectory = path;
		}

		/// <summary>
		/// Modifies an instance of OpenFileDialog, to open a given directory.
		/// </summary>
		/// <param name="fileDialog">OpenFileDialog instance to be modified.</param>
		/// <param name="path">Path to be opened by the OpenFileDialog</param>
		public static void SetOpenFileDialog(System.Windows.Forms.FileDialog fileDialog, System.IO.FileInfo path)
		{
			fileDialog.InitialDirectory = path.Directory.FullName;
		}

		/// <summary>
		/// Modifies an instance of OpenFileDialog, to open a given directory.
		/// </summary>
		/// <param name="fileDialog">OpenFileDialog instance to be modified.</param>
		/// <param name="path">Path to be opened by the OpenFileDialog.</param>
		public static void SetOpenFileDialog(System.Windows.Forms.FileDialog fileDialog, System.String path)
		{
			fileDialog.InitialDirectory = path;
		}

		///
		///  Use the following static methods to create instances of SaveFileDialog.
		///  By default, JFileChooser is converted as an OpenFileDialog, the following methods
		///  are provided to create file dialogs to save files.
		///	
		
		
		/// <summary>
		/// Creates a SaveFileDialog.
		/// </summary>		
		/// <returns>A new instance of SaveFileDialog.</returns>
		public static System.Windows.Forms.SaveFileDialog CreateSaveFileDialog()
		{
			System.Windows.Forms.SaveFileDialog temp_fileDialog = new System.Windows.Forms.SaveFileDialog();
			temp_fileDialog.InitialDirectory = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);			
			return temp_fileDialog;
		}

		/// <summary>
		/// Creates an SaveFileDialog open in a given path.
		/// </summary>
		/// <param name="path">Path to be opened by the SaveFileDialog.</param>
		/// <returns>A new instance of SaveFileDialog.</returns>
		public static System.Windows.Forms.SaveFileDialog CreateSaveFileDialog(System.IO.FileInfo path)
		{
			System.Windows.Forms.SaveFileDialog temp_fileDialog = new System.Windows.Forms.SaveFileDialog();
			temp_fileDialog.InitialDirectory = path.Directory.FullName;
			return temp_fileDialog;
		}

		/// <summary>
		/// Creates an SaveFileDialog open in a given path.
		/// </summary>
		/// <param name="path">Path to be opened by the SaveFileDialog.</param>
		/// <param name="directory">Directory to get the path from.</param>
		/// <returns>A new instance of SaveFileDialog.</returns>
		public static System.Windows.Forms.SaveFileDialog CreateSaveFileDialog(System.IO.FileInfo path, System.IO.DirectoryInfo directory)
		{
			System.Windows.Forms.SaveFileDialog temp_fileDialog = new System.Windows.Forms.SaveFileDialog();
			temp_fileDialog.InitialDirectory = path.Directory.FullName;
			return temp_fileDialog;
		}

		/// <summary>
		/// Creates a SaveFileDialog open in a given path.
		/// </summary>
		/// <param name="directory">Directory to get the path from.</param>
		/// <returns>A new instance of SaveFileDialog.</returns>
		public static System.Windows.Forms.SaveFileDialog CreateSaveFileDialog(System.IO.DirectoryInfo directory)
		{
			System.Windows.Forms.SaveFileDialog temp_fileDialog = new System.Windows.Forms.SaveFileDialog();
			temp_fileDialog.InitialDirectory = directory.FullName;
			return temp_fileDialog;
		}

		/// <summary>
		/// Creates an SaveFileDialog open in a given path.
		/// </summary>
		/// <param name="path">Path to be opened by the SaveFileDialog</param>
		/// <returns>A new instance of SaveFileDialog.</returns>
		public static System.Windows.Forms.SaveFileDialog CreateSaveFileDialog (System.String path)
		{
			System.Windows.Forms.SaveFileDialog temp_fileDialog = new System.Windows.Forms.SaveFileDialog();
			temp_fileDialog.InitialDirectory = path;
			return temp_fileDialog;
		}

		/// <summary>
		/// Creates an SaveFileDialog open in a given path.
		/// </summary>
		/// <param name="path">Path to be opened by the SaveFileDialog.</param>
		/// <param name="directory">Directory to get the path from.</param>
		/// <returns>A new instance of SaveFileDialog.</returns>
		public static System.Windows.Forms.SaveFileDialog CreateSaveFileDialog(System.String path, System.IO.DirectoryInfo directory)
		{
			System.Windows.Forms.SaveFileDialog temp_fileDialog = new System.Windows.Forms.SaveFileDialog();
			temp_fileDialog.InitialDirectory = path;
			return temp_fileDialog;
		}
	}
	/*******************************/
	/// <summary>
	/// This class supports basic functionality of the JOptionPane class.
	/// </summary>
	public class OptionPaneSupport 
	{
		/// <summary>
		/// This method finds the form which contains an specific control.
		/// </summary>
		/// <param name="control">The control which we need to find its form.</param>
		/// <returns>The form which contains the control</returns>
		public static System.Windows.Forms.Form GetFrameForComponent(System.Windows.Forms.Control control)
		{
			System.Windows.Forms.Form result = null;
			if (control == null)return null;
			if (control is System.Windows.Forms.Form)
				result = (System.Windows.Forms.Form)control;
			else if (control.Parent != null)
				result = GetFrameForComponent(control.Parent);
			return result;
		}

		/// <summary>
		/// This method finds the MDI container form which contains an specific control.
		/// </summary>
		/// <param name="control">The control which we need to find its MDI container form.</param>
		/// <returns>The MDI container form which contains the control.</returns>
		public static System.Windows.Forms.Form GetDesktopPaneForComponent(System.Windows.Forms.Control control)
		{
			System.Windows.Forms.Form result = null;
			if (control == null)return null;
			if (control.GetType().IsSubclassOf(typeof(System.Windows.Forms.Form)))
				if (((System.Windows.Forms.Form)control).IsMdiContainer)
					result = (System.Windows.Forms.Form)control;
				else if (((System.Windows.Forms.Form)control).IsMdiChild)
					result = GetDesktopPaneForComponent(((System.Windows.Forms.Form)control).MdiParent);
				else if (control.Parent != null)
					result = GetDesktopPaneForComponent(control.Parent);
			return result;
		}
		
		/// <summary>
		/// This method retrieves the message that is contained into the object that is sended by the user.
		/// </summary>
		/// <param name="control">The control which we need to find its form.</param>
		/// <returns>The form which contains the control</returns>
		public static System.String GetMessageForObject(System.Object message)
		{
			System.String result = "";
			if (message == null)
			  return result;
			else 
		 	  result = message.ToString();
			return result;
		}


		/// <summary>
		/// This method displays a dialog with a Yes, No, Cancel buttons and a question icon.
		/// </summary>
		/// <param name="parent">The component which will be the owner of the dialog.</param>
		/// <param name="message">The message to be displayed; if it isn't an String it displays the return value of the ToString() method of the object.</param>
		/// <returns>The integer value which represents the button pressed.</returns>
		public static int ShowConfirmDialog(System.Windows.Forms.Control parent, System.Object message)
		{
			return ShowConfirmDialog(parent, message,"Select an option...", (int)System.Windows.Forms.MessageBoxButtons.YesNoCancel,
				(int)System.Windows.Forms.MessageBoxIcon.Question);
		}

		/// <summary>
		/// This method displays a dialog with specific buttons and a question icon.
		/// </summary>
		/// <param name="parent">The component which will be the owner of the dialog.</param>
		/// <param name="message">The message to be displayed; if it isn't an String it displays the result value of the ToString() method of the object.</param>
		/// <param name="title">The title for the message dialog.</param>
		/// <param name="optiontype">The set of buttons to be displayed in the message box; defined by the MessageBoxButtons enumeration.</param>
		/// <returns>The integer value which represents the button pressed.</returns>
		public static int ShowConfirmDialog(System.Windows.Forms.Control parent, System.Object message,
			System.String title,int optiontype)
		{
			return ShowConfirmDialog(parent, message, title, optiontype, (int)System.Windows.Forms.MessageBoxIcon.Question);
		}

		/// <summary>
		/// This method displays a dialog with specific buttons and specific icon.
		/// </summary>
		/// <param name="parent">The component which will be the owner of the dialog.</param>
		/// <param name="message">The message to be displayed; if it isn't an String it displays the return value of the ToString() method of the object.</param>
		/// <param name="title">The title for the message dialog.</param>
		/// <param name="optiontype">The set of buttons to be displayed in the message box; defined by the MessageBoxButtons enumeration.</param>
		/// <param name="messagetype">The messagetype defines the icon to be displayed in the message box.</param>
		/// <returns>The integer value which represents the button pressed.</returns>
		public static int ShowConfirmDialog(System.Windows.Forms.Control parent, System.Object message,
			System.String title, int optiontype, int messagetype)
		{
			return (int)System.Windows.Forms.MessageBox.Show(GetFrameForComponent(parent), GetMessageForObject(message), title,
				(System.Windows.Forms.MessageBoxButtons)optiontype, (System.Windows.Forms.MessageBoxIcon)messagetype);
		}

		/// <summary>
		/// This method displays a simple MessageBox.
		/// </summary>
		/// <param name="parent">The component which will be the owner of the dialog.</param>
		/// <param name="message">The message to be displayed; if it isn't an String it displays result value of the ToString() method of the object.</param>
		public static void ShowMessageDialog(System.Windows.Forms.Control parent, System.Object message)
		{
			ShowMessageDialog(parent, message, "Message", (int)System.Windows.Forms.MessageBoxIcon.Information);
		}

		/// <summary>
		/// This method displays a simple MessageBox with a specific icon.
		/// </summary>
		/// <param name="parent">The component which will be the owner of the dialog.</param>
		/// <param name="message">The message to be displayed; if it isn't an String it displays result value of the ToString() method of the object.</param>
		/// <param name="title">The title for the message dialog.</param>
		/// <param name="messagetype">The messagetype defines the icon to be displayed in the message box.</param>
		public static void ShowMessageDialog(System.Windows.Forms.Control parent, System.Object message,
			System.String title, int messagetype)
		{
			System.Windows.Forms.MessageBox.Show(GetFrameForComponent(parent), GetMessageForObject(message), title,
				System.Windows.Forms.MessageBoxButtons.OK, (System.Windows.Forms.MessageBoxIcon)messagetype);
		}
	}


	/*******************************/
	/// <summary>
	/// Gets the selected items in a ListBox instance.
	/// </summary>
	/// <param name="listbox">A listbox to get its selected items.</param>
	/// <returns>An object array with the selected items.</returns>
	public static System.Object[] GetSelectedItems(System.Windows.Forms.ListBox listbox)
	{
		System.Object[] selectedvalues = new System.Object[listbox.SelectedItems.Count];
		listbox.SelectedItems.CopyTo(selectedvalues, 0);
		return selectedvalues;
	}


	/*******************************/
	/// <summary>
	/// The SplitterPanel its a panel with two controls separated by a movable splitter.
	/// </summary>
	public class SplitterPanelSupport : System.Windows.Forms.Panel
	{
		private System.Windows.Forms.Control firstControl;
		private System.Windows.Forms.Control secondControl;
		private System.Windows.Forms.Splitter splitter;
		private System.Windows.Forms.Orientation orientation;
		private int splitterSize;
		private int splitterLocation;
		private int lastSplitterLocation;

		//Default controls
		private System.Windows.Forms.Control defaultFirstControl;
		private System.Windows.Forms.Control defaultSecondControl;

		/// <summary>
		/// Creates a SplitterPanel with Horizontal orientation and two buttons as the default
		/// controls. The default size of the splitter is set to 5.
		/// </summary>
		public SplitterPanelSupport() : base()
		{
			System.Windows.Forms.Button button1 = new System.Windows.Forms.Button();
			System.Windows.Forms.Button button2 = new System.Windows.Forms.Button();
			button1.Text = "button1";
			button2.Text = "button2";
				
			this.lastSplitterLocation = -1;
			this.orientation = System.Windows.Forms.Orientation.Horizontal;
			this.splitterSize = 5;

			this.defaultFirstControl  = button1;
			this.defaultSecondControl = button2;
			this.firstControl  = this.defaultFirstControl;
			this.secondControl = this.defaultSecondControl;
			this.splitterLocation = this.firstControl.Size.Width;
			this.splitter = new System.Windows.Forms.Splitter();
			this.SuspendLayout();

			//
			// panel1
			//
			this.Controls.Add(this.splitter);
			this.Controls.Add(this.firstControl);
			this.Controls.Add(this.secondControl);
				
			// 
			// firstControl
			// 
			this.firstControl.Dock = System.Windows.Forms.DockStyle.Left;
			this.firstControl.Name = "firstControl";
			this.firstControl.TabIndex = 0;
				
			// 
			// secondControl
			//
			this.secondControl.Name = "secondControl";
			this.secondControl.TabIndex = 1;
			this.secondControl.Size = new System.Drawing.Size((this.Size.Width - this.firstControl.Size.Width) + this.splitterSize, this.Size.Height);
			this.secondControl.Location = new System.Drawing.Point((this.firstControl.Location.X + this.firstControl.Size.Width + this.splitterSize), 0);

			// 
			// splitter
			//			
			this.splitter.Name = "splitter";
			this.splitter.TabIndex = 2;
			this.splitter.TabStop = false;
			this.splitter.MinExtra = 10;
			this.splitter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.splitter.Size = new System.Drawing.Size(this.splitterSize, this.Size.Height);
			this.splitter.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(splitter_SplitterMoved);
				
			this.SizeChanged += new System.EventHandler(SplitterPanel_SizeChanged);
			this.ResumeLayout(false);
		}

		/// <summary>
		/// Creates a new SplitterPanelSupport with two buttons as default controls and the specified
		/// splitter orientation.
		/// </summary>
		/// <param name="newOrientation">The orientation of the SplitterPanel.</param>
		public SplitterPanelSupport(int newOrientation) : this()
		{
			this.SplitterOrientation = (System.Windows.Forms.Orientation) newOrientation;
		}

		/// <summary>
		/// Creates a new SplitterPanelSupport with the specified controls and orientation.
		/// </summary>
		/// <param name="newOrientation">The orientation of the SplitterPanel.</param>
		/// <param name="first">The first control of the panel, left-top control.</param>
		/// <param name="second">The second control of the panel, right-botton control.</param>
		public SplitterPanelSupport(int newOrientation, System.Windows.Forms.Control first, System.Windows.Forms.Control second) : this(newOrientation)
		{
			this.FirstControl  = first;
			this.SecondControl = second;
		}


		/// <summary>
		/// Creates a new SplitterPanelSupport with the specified controls and orientation.
		/// </summary>		
		/// <param name="first">The first control of the panel, left-top control.</param>
		/// <param name="second">The second control of the panel, right-botton control.</param>
		public SplitterPanelSupport(System.Windows.Forms.Control first, System.Windows.Forms.Control second) : this()
		{
			this.FirstControl  = first;
			this.SecondControl = second;
		}

		/// <summary>
		/// Adds a control to the SplitterPanel in the first available position.
		/// </summary>		
		/// <param name="control">The control to by added.</param>
		public void Add(System.Windows.Forms.Control control)
		{
			if(FirstControl == defaultFirstControl)
				FirstControl = control;
			else if(SecondControl == defaultSecondControl) 
				SecondControl = control;
		}

		/// <summary>
		/// Adds a control to the SplitterPanel in the specified position.
		/// </summary>		
		/// <param name="control">The control to by added.</param>
		/// <param name="position">The position to add the control in the SpliterPanel.</param>
		public void Add(System.Windows.Forms.Control control, SpliterPosition position)
		{
			if(position == SpliterPosition.First)
				FirstControl = control;
			else if(position == SpliterPosition.Second) 
				SecondControl = control;
		}

		/// <summary>
		/// Defines the possible positions of a control in a SpliterPanel.
		/// </summary>		
		public enum SpliterPosition
		{
			First,
			Second,
		}

		/// <summary>
		/// Gets the specified component.
		/// </summary>
		/// <param name="name">the name of the part of the component to get: "nw": first control, 
		/// "se": second control, "splitter": splitter.</param>
		/// <returns>returns the specified component.</returns>
		public virtual System.Windows.Forms.Control GetComponent(System.String name)
		{
			if (name == "nw")
				return this.FirstControl;
			else if (name == "se")
				return this.SecondControl;
			else if (name == "splitter")
				return this.splitter;
			else return null;
		}

		/// <summary>
		/// First control of the panel. When orientation is Horizontal then this is the left control
		/// when the orientation is Vertical then this is the top control.
		/// </summary>
		public virtual System.Windows.Forms.Control FirstControl
		{
			get
			{
				return this.firstControl;
			}
			set
			{
				this.Controls.Remove(this.firstControl);
				if (this.orientation == System.Windows.Forms.Orientation.Horizontal)
					value.Dock = System.Windows.Forms.DockStyle.Left;
				else
					value.Dock = System.Windows.Forms.DockStyle.Top;
				value.Size = this.firstControl.Size;
				this.firstControl = value;
				this.Controls.Add(this.firstControl);
			}
		}

		/// <summary>
		/// The second control of the panel. Right control when the panel is Horizontal oriented and
		/// botton control when the SplitterPanel orientation is Vertical.
		/// </summary>
		public virtual System.Windows.Forms.Control SecondControl
		{
			get
			{
				return this.secondControl;
			}
			set
			{
				this.Controls.Remove(this.secondControl);
				value.Size = this.secondControl.Size;
				value.Location = this.secondControl.Location;
				this.secondControl = value;
				this.Controls.Add(this.secondControl);
			}
		}

		/// <summary>
		/// The orientation of the SplitterPanel. Specifies how the controls are aligned.
		/// Left to right using Horizontal orientation or top to botton with Vertical orientation.
		/// </summary>
		public virtual System.Windows.Forms.Orientation SplitterOrientation
		{
			get
			{
				return this.orientation;
			}
			set
			{
				if (value != this.orientation)
				{
					this.orientation = value;
					if (value == System.Windows.Forms.Orientation.Vertical)
					{
						int lastWidth = this.firstControl.Size.Width;
						this.firstControl.Dock = System.Windows.Forms.DockStyle.Top;
						this.firstControl.Size = new System.Drawing.Size(this.Width, lastWidth);
						this.splitter.Dock = System.Windows.Forms.DockStyle.Top;
					}
					else
					{
						int lastHeight = this.firstControl.Size.Height;
						this.firstControl.Dock = System.Windows.Forms.DockStyle.Left;
						this.firstControl.Size = new System.Drawing.Size(lastHeight, this.Height);
						this.splitter.Dock = System.Windows.Forms.DockStyle.Left;
					}
					this.ResizeSecondControl();
				}
			}
		}

		/// <summary>
		/// Specifies the location of the Splitter in the panel.
		/// </summary>
		public virtual int SplitterLocation
		{
			get
			{
				return this.splitterLocation;
			}
			set
			{
				if (this.orientation == System.Windows.Forms.Orientation.Horizontal)
					this.firstControl.Size = new System.Drawing.Size(value, this.Height);
				else
					this.firstControl.Size = new System.Drawing.Size(this.Width, value);					
				this.ResizeSecondControl();
				this.lastSplitterLocation = this.splitterLocation;
				this.splitterLocation = value;
			}
		}

		/// <summary>
		/// The last location of the splitter on the panel.
		/// </summary>
		public virtual int LastSplitterLocation
		{
			get
			{
				return this.lastSplitterLocation;
			}
			set
			{
				this.lastSplitterLocation = value;
			}
		}

		/// <summary>
		/// Specifies the size of the splitter divider.
		/// </summary>
		public virtual int SplitterSize
		{
			get
			{
				return this.splitterSize;
			}
			set
			{
				this.splitterSize = value;
				if (this.orientation == System.Windows.Forms.Orientation.Horizontal)
					this.splitter.Size = new System.Drawing.Size(this.splitterSize, this.Size.Height);
				else
					this.splitter.Size = new System.Drawing.Size(this.Size.Width, this.splitterSize);
				this.ResizeSecondControl();
			}
		}

		/// <summary>
		/// The minimum location of the splitter on the panel.
		/// </summary>
		/// <returns>The minimum location value for the splitter.</returns>
		public virtual int GetMinimumLocation()
		{
			return this.splitter.MinSize;
		}

		/// <summary>
		/// The maximum location of the splitter on the panel.
		/// </summary>
		/// <returns>The maximum location value for the splitter.</returns>
		public virtual int GetMaximumLocation()
		{
			if (this.orientation == System.Windows.Forms.Orientation.Horizontal)
				return this.Width - ( this.SplitterSize / 2 );
			else
				return this.Height - ( this.SplitterSize / 2 );
		}

		/// <summary>
		/// Adds a control to splitter panel.
		/// </summary>
		/// <param name="controlToAdd">The control to add.</param>
		/// <param name="dockStyle">The dock style for the control, left-top, or botton-right.</param>
		/// <param name="index">The index of the control in the panel control list.</param>
		protected virtual void AddControl(System.Windows.Forms.Control controlToAdd, System.Object dockStyle, int index)
		{
			if (dockStyle is System.String)
			{
				System.String dock = (System.String)dockStyle;
				if (dock == "botton" || dock == "right")
					this.SecondControl = controlToAdd;
				else if (dock == "top" || dock == "left")
					this.FirstControl  = controlToAdd;
				else
					throw new System.ArgumentException("Cannot add control: unknown constraint: " + dockStyle.ToString());
				this.Controls.SetChildIndex(controlToAdd, index);
			}
			else
				throw new System.ArgumentException("Cannot add control: unknown constraint: " + dockStyle.ToString());
		}

		/// <summary>
		/// Removes the specified control from the panel.
		/// </summary>
		/// <param name="controlToRemove">The control to remove.</param>
		public virtual void RemoveControl(System.Windows.Forms.Control controlToRemove)
		{
			if (this.Contains(controlToRemove))
			{
				this.Controls.Remove(controlToRemove);
				if (this.firstControl == controlToRemove)
					this.secondControl.Dock = System.Windows.Forms.DockStyle.Fill;
				else
					this.firstControl.Dock = System.Windows.Forms.DockStyle.Fill;
			}
		}

		/// <summary>
		/// Remove the control identified by the specified index.
		/// </summary>
		/// <param name="index">The index of the control to remove.</param>
		public virtual void RemoveControl(int index)
		{
			try 
			{
				this.Controls.RemoveAt(index);
				if (this.firstControl != null)
					if (this.Controls.Contains(this.firstControl))
						this.firstControl.Dock = System.Windows.Forms.DockStyle.Fill;
					else if (this.secondControl != null && (this.Controls.Contains(this.secondControl)))
						this.secondControl.Dock = System.Windows.Forms.DockStyle.Fill;
			} // Compatibility with the conversion assistant.
			catch (System.ArgumentOutOfRangeException)
			{
				throw new System.IndexOutOfRangeException("No such child: " + index);
			}
		}
			
		/// <summary>
		/// Changes the location of the splitter in the panel as a percentage of the panel's size.
		/// </summary>
		/// <param name="proportion">The percentage from 0.0 to 1.0.</param>
		public virtual void SetLocationProportional(double proportion)
		{
			if ((proportion > 0.0) && (proportion < 1.0))
				this.SplitterLocation = (int)((this.orientation == System.Windows.Forms.Orientation.Horizontal) ? (proportion * this.Width) : (proportion * this.Height));
			else
				throw new System.ArgumentException("Proportional location must be between 0.0 and 1.0");
		}

		private void splitter_SplitterMoved(System.Object sender, System.Windows.Forms.SplitterEventArgs e)
		{
			this.lastSplitterLocation = this.splitterLocation;
			if (this.orientation == System.Windows.Forms.Orientation.Horizontal)
				this.splitterLocation = this.firstControl.Width;
			else
				this.splitterLocation = this.firstControl.Height;
			this.ResizeSecondControl();
		}

		private void SplitterPanel_SizeChanged(System.Object sender, System.EventArgs e)
		{
			this.ResizeSecondControl();
		}

		private void ResizeSecondControl()
		{
			if (this.orientation == System.Windows.Forms.Orientation.Horizontal)
			{
				this.secondControl.Size = new System.Drawing.Size((this.Width - (this.firstControl.Size.Width + this.splitterSize)), this.Size.Height);
				this.secondControl.Location = new System.Drawing.Point((this.firstControl.Size.Width + this.splitterSize), 0);
			}
			else
			{
				this.secondControl.Size = new System.Drawing.Size(this.Size.Width, (this.Size.Height - (this.firstControl.Size.Height + this.splitterSize)));
				this.secondControl.Location = new System.Drawing.Point(0, (this.firstControl.Size.Height + this.splitterSize));
			}
		}
	}


	/*******************************/
	/// <summary>
	/// Support class used to handle threads
	/// </summary>
	public class ThreadClass : IThreadRunnable
	{
		/// <summary>
		/// The instance of System.Threading.Thread
		/// </summary>
		private System.Threading.Thread threadField;
	      
		/// <summary>
		/// Initializes a new instance of the ThreadClass class
		/// </summary>
		public ThreadClass()
		{
			threadField = new System.Threading.Thread(new System.Threading.ThreadStart(Run));
		}
	 
		/// <summary>
		/// Initializes a new instance of the Thread class.
		/// </summary>
		/// <param name="Name">The name of the thread</param>
		public ThreadClass(System.String Name)
		{
			threadField = new System.Threading.Thread(new System.Threading.ThreadStart(Run));
			this.Name = Name;
		}
	      
		/// <summary>
		/// Initializes a new instance of the Thread class.
		/// </summary>
		/// <param name="Start">A ThreadStart delegate that references the methods to be invoked when this thread begins executing</param>
		public ThreadClass(System.Threading.ThreadStart Start)
		{
			threadField = new System.Threading.Thread(Start);
		}
	 
		/// <summary>
		/// Initializes a new instance of the Thread class.
		/// </summary>
		/// <param name="Start">A ThreadStart delegate that references the methods to be invoked when this thread begins executing</param>
		/// <param name="Name">The name of the thread</param>
		public ThreadClass(System.Threading.ThreadStart Start, System.String Name)
		{
			threadField = new System.Threading.Thread(Start);
			this.Name = Name;
		}
	      
		/// <summary>
		/// This method has no functionality unless the method is overridden
		/// </summary>
		public virtual void Run()
		{
		}
	      
		/// <summary>
		/// Causes the operating system to change the state of the current thread instance to ThreadState.Running
		/// </summary>
		public virtual void Start()
		{
			threadField.Start();
		}
	      
		/// <summary>
		/// Interrupts a thread that is in the WaitSleepJoin thread state
		/// </summary>
		public virtual void Interrupt()
		{
			threadField.Interrupt();
		}
	      
		/// <summary>
		/// Gets the current thread instance
		/// </summary>
		public System.Threading.Thread Instance
		{
			get
			{
				return threadField;
			}
			set
			{
				threadField = value;
			}
		}
	      
		/// <summary>
		/// Gets or sets the name of the thread
		/// </summary>
		public System.String Name
		{
			get
			{
				return threadField.Name;
			}
			set
			{
				if (threadField.Name == null)
					threadField.Name = value; 
			}
		}
	      
		/// <summary>
		/// Gets or sets a value indicating the scheduling priority of a thread
		/// </summary>
		public System.Threading.ThreadPriority Priority
		{
			get
			{
				return threadField.Priority;
			}
			set
			{
				threadField.Priority = value;
			}
		}
	      
		/// <summary>
		/// Gets a value indicating the execution status of the current thread
		/// </summary>
		public bool IsAlive
		{
			get
			{
				return threadField.IsAlive;
			}
		}
	      
		/// <summary>
		/// Gets or sets a value indicating whether or not a thread is a background thread.
		/// </summary>
		public bool IsBackground
		{
			get
			{
				return threadField.IsBackground;
			} 
			set
			{
				threadField.IsBackground = value;
			}
		}
	      
		/// <summary>
		/// Blocks the calling thread until a thread terminates
		/// </summary>
		public void Join()
		{
			threadField.Join();
		}
	      
		/// <summary>
		/// Blocks the calling thread until a thread terminates or the specified time elapses
		/// </summary>
		/// <param name="MiliSeconds">Time of wait in milliseconds</param>
		public void Join(long MiliSeconds)
		{
			lock(this)
			{
				threadField.Join(new System.TimeSpan(MiliSeconds * 10000));
			}
		}
	      
		/// <summary>
		/// Blocks the calling thread until a thread terminates or the specified time elapses
		/// </summary>
		/// <param name="MiliSeconds">Time of wait in milliseconds</param>
		/// <param name="NanoSeconds">Time of wait in nanoseconds</param>
		public void Join(long MiliSeconds, int NanoSeconds)
		{
			lock(this)
			{
				threadField.Join(new System.TimeSpan(MiliSeconds * 10000 + NanoSeconds * 100));
			}
		}
	      
		/// <summary>
		/// Resumes a thread that has been suspended
		/// </summary>
		public void Resume()
		{
			threadField.Resume();
		}
	      
		/// <summary>
		/// Raises a ThreadAbortException in the thread on which it is invoked, 
		/// to begin the process of terminating the thread. Calling this method 
		/// usually terminates the thread
		/// </summary>
		public void Abort()
		{
			threadField.Abort();
		}
	      
		/// <summary>
		/// Raises a ThreadAbortException in the thread on which it is invoked, 
		/// to begin the process of terminating the thread while also providing
		/// exception information about the thread termination. 
		/// Calling this method usually terminates the thread.
		/// </summary>
		/// <param name="stateInfo">An object that contains application-specific information, such as state, which can be used by the thread being aborted</param>
		public void Abort(System.Object stateInfo)
		{
			lock(this)
			{
				threadField.Abort(stateInfo);
			}
		}
	      
		/// <summary>
		/// Suspends the thread, if the thread is already suspended it has no effect
		/// </summary>
		public void Suspend()
		{
			threadField.Suspend();
		}
	      
		/// <summary>
		/// Obtain a String that represents the current Object
		/// </summary>
		/// <returns>A String that represents the current Object</returns>
		public override System.String ToString()
		{
			return "Thread[" + Name + "," + Priority.ToString() + "," + "" + "]";
		}
	     
		/// <summary>
		/// Gets the currently running thread
		/// </summary>
		/// <returns>The currently running thread</returns>
		public static ThreadClass Current()
		{
			ThreadClass CurrentThread = new ThreadClass();
			CurrentThread.Instance = System.Threading.Thread.CurrentThread;
			return CurrentThread;
		}
	}


	/*******************************/
	/// <summary>
	/// SupportClass for the HashSet class.
	/// </summary>
	[Serializable]
	public class HashSetSupport : System.Collections.ArrayList, SetSupport
	{
		public HashSetSupport() : base()
		{	
		}

		public HashSetSupport(System.Collections.ICollection c) 
		{
			this.AddAll(c);
		}

		public HashSetSupport(int capacity) : base(capacity)
		{
		}

		/// <summary>
		/// Adds a new element to the ArrayList if it is not already present.
		/// </summary>		
		/// <param name="obj">Element to insert to the ArrayList.</param>
		/// <returns>Returns true if the new element was inserted, false otherwise.</returns>
		new public virtual bool Add(System.Object obj)
		{
			bool inserted;

			if ((inserted = this.Contains(obj)) == false)
			{
				base.Add(obj);
			}

			return !inserted;
		}

		/// <summary>
		/// Adds all the elements of the specified collection that are not present to the list.
		/// </summary>
		/// <param name="c">Collection where the new elements will be added</param>
		/// <returns>Returns true if at least one element was added, false otherwise.</returns>
		public bool AddAll(System.Collections.ICollection c)
		{
			System.Collections.IEnumerator e = new System.Collections.ArrayList(c).GetEnumerator();
			bool added = false;

			while (e.MoveNext() == true)
			{
				if (this.Add(e.Current) == true)
					added = true;
			}

			return added;
		}
		
		/// <summary>
		/// Returns a copy of the HashSet instance.
		/// </summary>		
		/// <returns>Returns a shallow copy of the current HashSet.</returns>
		public override System.Object Clone()
		{
			return base.MemberwiseClone();
		}
	}


	/*******************************/
	/// <summary>
	/// Represents a collection ob objects that contains no duplicate elements.
	/// </summary>	
	public interface SetSupport : System.Collections.ICollection, System.Collections.IList
	{
		/// <summary>
		/// Adds a new element to the Collection if it is not already present.
		/// </summary>
		/// <param name="obj">The object to add to the collection.</param>
		/// <returns>Returns true if the object was added to the collection, otherwise false.</returns>
		new bool Add(System.Object obj);

		/// <summary>
		/// Adds all the elements of the specified collection to the Set.
		/// </summary>
		/// <param name="c">Collection of objects to add.</param>
		/// <returns>true</returns>
		bool AddAll(System.Collections.ICollection c);
	}


	/*******************************/
	/// <summary>
	/// SupportClass for the SortedSet interface.
	/// </summary>
	public interface SortedSetSupport : SetSupport
	{
		/// <summary>
		/// Returns a portion of the list whose elements are less than the limit object parameter.
		/// </summary>
		/// <param name="l">The list where the portion will be extracted.</param>
		/// <param name="limit">The end element of the portion to extract.</param>
		/// <returns>The portion of the collection whose elements are less than the limit object parameter.</returns>
		SortedSetSupport HeadSet(System.Object limit);

		/// <summary>
		/// Returns a portion of the list whose elements are greater that the lowerLimit parameter less than the upperLimit parameter.
		/// </summary>
		/// <param name="l">The list where the portion will be extracted.</param>
		/// <param name="limit">The start element of the portion to extract.</param>
		/// <param name="limit">The end element of the portion to extract.</param>
		/// <returns>The portion of the collection.</returns>
		SortedSetSupport SubSet(System.Object lowerLimit, System.Object upperLimit);

		/// <summary>
		/// Returns a portion of the list whose elements are greater than the limit object parameter.
		/// </summary>
		/// <param name="l">The list where the portion will be extracted.</param>
		/// <param name="limit">The start element of the portion to extract.</param>
		/// <returns>The portion of the collection whose elements are greater than the limit object parameter.</returns>
		SortedSetSupport TailSet(System.Object limit);
	}


	/*******************************/
	/// <summary>
	/// SupportClass for the TreeSet class.
	/// </summary>
	[Serializable]
	public class TreeSetSupport : System.Collections.ArrayList, SetSupport, SortedSetSupport
	{
		private System.Collections.IComparer comparator = System.Collections.Comparer.Default;

		public TreeSetSupport() : base()
		{
		}

		public TreeSetSupport(System.Collections.ICollection c) : base()
		{
			this.AddAll(c);
		}

		public TreeSetSupport(System.Collections.IComparer c) : base()
		{
			this.comparator = c;
		}

		/// <summary>
		/// Gets the IComparator object used to sort this set.
		/// </summary>
		public System.Collections.IComparer Comparator
		{
			get
			{
				return this.comparator;
			}
		}

		/// <summary>
		/// Adds a new element to the ArrayList if it is not already present and sorts the ArrayList.
		/// </summary>
		/// <param name="obj">Element to insert to the ArrayList.</param>
		/// <returns>TRUE if the new element was inserted, FALSE otherwise.</returns>
		new public bool Add(System.Object obj)
		{
			bool inserted;
			if ((inserted = this.Contains(obj)) == false)
			{
				base.Add(obj);
				this.Sort(this.comparator);
			}
			return !inserted;
		}

		/// <summary>
		/// Adds all the elements of the specified collection that are not present to the list.
		/// </summary>		
		/// <param name="c">Collection where the new elements will be added</param>
		/// <returns>Returns true if at least one element was added to the collection.</returns>
		public bool AddAll(System.Collections.ICollection c)
		{
			System.Collections.IEnumerator e = new System.Collections.ArrayList(c).GetEnumerator();
			bool added = false;
			while (e.MoveNext() == true)
			{
				if (this.Add(e.Current) == true)
					added = true;
			}
			this.Sort(this.comparator);
			return added;
		}

		/// <summary>
		/// Determines whether an element is in the the current TreeSetSupport collection. The IComparer defined for 
		/// the current set will be used to make comparisons between the elements already inserted in the collection and 
		/// the item specified.
		/// </summary>
		/// <param name="item">The object to be locatet in the current collection.</param>
		/// <returns>true if item is found in the collection; otherwise, false.</returns>
		public override bool Contains(System.Object item)
		{
			System.Collections.IEnumerator tempEnumerator = this.GetEnumerator();
			while (tempEnumerator.MoveNext())
				if (this.comparator.Compare(tempEnumerator.Current, item) == 0)
					return true;
			return false;
		}

		/// <summary>
		/// Returns a portion of the list whose elements are less than the limit object parameter.
		/// </summary>
		/// <param name="limit">The end element of the portion to extract.</param>
		/// <returns>The portion of the collection whose elements are less than the limit object parameter.</returns>
		public SortedSetSupport HeadSet(System.Object limit)
		{
			SortedSetSupport newList = new TreeSetSupport();
			for (int i = 0; i < this.Count; i++)
			{
				if (this.comparator.Compare(this[i], limit) >= 0)
					break;
				newList.Add(this[i]);
			}
			return newList;
		}

		/// <summary>
		/// Returns a portion of the list whose elements are greater that the lowerLimit parameter less than the upperLimit parameter.
		/// </summary>
		/// <param name="lowerLimit">The start element of the portion to extract.</param>
		/// <param name="upperLimit">The end element of the portion to extract.</param>
		/// <returns>The portion of the collection.</returns>
		public SortedSetSupport SubSet(System.Object lowerLimit, System.Object upperLimit)
		{
			SortedSetSupport newList = new TreeSetSupport();
			int i = 0;
			while (this.comparator.Compare(this[i], lowerLimit) < 0)
				i++;
			for (; i < this.Count; i++)
			{
				if (this.comparator.Compare(this[i], upperLimit) >= 0)
					break;
				newList.Add(this[i]);
			}
			return newList;
		}

		/// <summary>
		/// Returns a portion of the list whose elements are greater than the limit object parameter.
		/// </summary>
		/// <param name="limit">The start element of the portion to extract.</param>
		/// <returns>The portion of the collection whose elements are greater than the limit object parameter.</returns>
		public SortedSetSupport TailSet(System.Object limit)
		{
			SortedSetSupport newList = new TreeSetSupport();
			int i = 0;
			while (this.comparator.Compare(this[i], limit) < 0)
				i++;
			for (; i < this.Count; i++)
				newList.Add(this[i]);
			return newList;
		}
	}


}
