﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.Distributions;
using Faker;
using UMD.HCIL.PiccoloX.Util.PStyledTextHelpers;
using UMD.HCIL.PiccoloX.Nodes;

namespace zoom.Generator
{
    public class Section
    {
        public int ParagraphLength { get; protected set; }
        public int ParagraphVariance { get; protected set; }

        public SectionSelector NextSection { get; protected set; }

        public Style Style { get; protected set; }

        protected static Random Rand = new Random();

        public Section(Font font, SectionSelector nextSection, int paraLength) : this(font, Color.FromArgb(0, 0, 0), nextSection, paraLength) { }

        public Section(Font font, Color color, SectionSelector nextSection, int paraLength) : this(new Style(font, color), nextSection, paraLength) { }

        public Section(Style style, SectionSelector nextSection, int paraLength)
        {
            Style = style;
            NextSection = nextSection;
            ParagraphLength = paraLength;
        }

        public Section getNext()
        {
            return NextSection.Select();
        }

        public Section generate(Document target)
        {
            return generate(target.Pages[target.Pages.Length - 1]);
        }

        public Section generate(Page target)
        {
            return generate(target.Model);
        }

        public Section generate(Model target)
        {
            //Add the styled text
            target.Select(target.TextLength, 0);
            Style.ApplyStyle(target);
            target.SelectedText = generateText();

            //Decide which type of paragraph goes next
            return getNext();
        }

        public string generateText()
        {
            int pLength = (int)Math.Ceiling(Normal.Sample((double)ParagraphLength, (double)ParagraphVariance));
            return String.Join(" ", Lorem.Sentences(pLength)) + "\n";

        }
    }
}
