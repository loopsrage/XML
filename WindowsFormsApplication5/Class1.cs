using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using System.Data;

namespace WindowsFormsApplication5
{
    public class ParsedXML
    {
        public XmlAttributeCollection ObjectAttributes { get; set; }
        public XmlNodeType NodeType { get; set; }
        public string BasePath { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public string ObjectParent { get; set; }
        public string LocalName { get; set; }
        public int ChildCount { get; set; }
        public ParsedXML()
        {

        }
    }
    public class XMLParser
    {
        public List<Dictionary<string, ParsedXML>> OutputObject = new List<Dictionary<string, ParsedXML>>();
        public XmlSerializer serializer = new XmlSerializer(typeof(XmlNode));
        public void ParseXML(string InputXML)
        {
            Dictionary<string, ParsedXML> TempO = new Dictionary<string, ParsedXML>();
            ParsedXML TempPX = new ParsedXML();
            if (InputXML != null)
            {
                using (XmlReader reader = XmlReader.Create(new StringReader(InputXML)))
                {
                    dynamic XN = serializer.Deserialize(reader);
                    XmlNode XNN = XN as XmlNode;
                    if (XNN != null)
                    {
                        ParseXML(XNN);
                    }
                }
            }
        }
        public void ParseXML(XmlNode InputXML)
        {
            Dictionary<string, ParsedXML> TempO = new Dictionary<string, ParsedXML>();
            ParsedXML TempPX = new ParsedXML();
            if (InputXML != null)
            {
                XmlNode XNN = InputXML;
                TempPX.NodeType = XNN.NodeType;
                TempPX.ObjectAttributes = XNN.Attributes;
                TempPX.BasePath = XNN.BaseURI;
                TempPX.Name = XNN.Name;
                TempPX.LocalName = XNN.LocalName;
                TempPX.ChildCount = XNN.ChildNodes.Count;
                if (XNN.ParentNode != null)
                {
                    TempPX.ObjectParent = XNN.ParentNode.Name;
                }

                if (XNN.FirstChild != null)
                {
                    if (XNN.FirstChild.NodeType.ToString().ToLower() == "text")
                    {
                        TempPX.Value = XNN.FirstChild.Value;
                    }
                    else
                    {
                        ParseXML(XNN.FirstChild);
                    }

                }
                TempO.Add(TempPX.LocalName, TempPX);
                if (TempPX.NodeType.ToString().ToLower() != "whitespace")
                {
                    OutputObject.Add(TempO);
                }
                if (XNN.NextSibling != null)
                {
                    ParseXML(XNN.NextSibling);
                }
            }
        }
        public XMLParser()
        {

        }
    }
    public class DynamicForms
    {
        public List<TextBox> CreateForms(List<Dictionary<string, ParsedXML>> XMLResults)
        {
            List<TextBox> TBL = new List<TextBox>();
            Point Orig = new Point(0,0);
            foreach (Dictionary<string, ParsedXML> Q in XMLResults)
            {
                foreach (string NS in Q.Keys)
                {
                    ParsedXML O = Q[NS];

                    TextBox TN = new TextBox();
                    TextBox TV = new TextBox();

                    TN.Name = O.LocalName;
                    TN.Text = O.LocalName;
                    TN.Location = Orig;

                    TV.Name = "V"+O.LocalName;
                    TV.Text = O.Value;

                    if (O.Value != null)
                    {
                        TV.Location = new Point(Orig.X+TN.Width,Orig.Y);
                        TBL.Add(TV);
                    }
                    else if(O.Value == null)
                    {
                        Orig.X -= TN.Width;
                        TN.Location = Orig;
                        Orig.X += TN.Width;

                    }
                    TBL.Add(TN);
                    Orig.Y += 25;
                }
            }
            return TBL;
        }
    }
    public class FormGenerate
    {
        public ContainerControl Container = new ContainerControl();
        public RichTextBox XMLInput = new RichTextBox();
        public Button ParseXML = new Button();
        public Label XMLMessage = new Label();
        public XMLParser XMLP = new XMLParser();
        public DynamicForms DF = new DynamicForms();
        public FormGenerate()
        {
            // Container Settings
            Container.Location = new Point(0, 0);
            Container.Width = 500;
            Container.Height = 500;
            Container.Controls.Add(XMLInput);
            Container.Controls.Add(ParseXML);
            Container.Controls.Add(XMLMessage);
            Container.AutoScroll = true;
            // RichTextBox Settings
            XMLInput.Location = new Point(Container.Width, Container.Height / 2);
            XMLInput.Height = 300;
            XMLInput.Width = 250;
            XMLInput.Text = @"<?xml version='1.0genre
<bookstore>
        <book publicationdateautobiography' '?>='='1981-03-22' ISBN='1-861003-11-0'>
            <title>The Autobiography of Benjamin Franklin</title>
            <author>
                <first-name>Benjamin</first-name>
                <last-name>Franklin</last-name>
            </author>
            <price>8.99</price>
        </book>
    </bookstore>";
            // Button Settings
            ParseXML.Location = new Point(400, 30);
            ParseXML.AutoSize = true;
            ParseXML.Click += ParseXML_Click;
            ParseXML.Text = "Parse XML";
            // Label Settings
            XMLMessage.Location = new Point(400, 10);
            XMLMessage.AutoSize = true;
            XMLMessage.Text = "Test";
        }
        private void ParseXML_Click(object sender, EventArgs e)
        {
            XMLP.ParseXML(XMLInput.Text);
            foreach (var E in DF.CreateForms(XMLP.OutputObject))
            {
                Container.Controls.Add(E);
            }
        }
    }
}