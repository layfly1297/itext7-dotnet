/*
This file is part of the iText (R) project.
Copyright (c) 1998-2019 iText Group NV
Authors: iText Software.

This program is free software; you can redistribute it and/or modify
it under the terms of the GNU Affero General Public License version 3
as published by the Free Software Foundation with the addition of the
following permission added to Section 15 as permitted in Section 7(a):
FOR ANY PART OF THE COVERED WORK IN WHICH THE COPYRIGHT IS OWNED BY
ITEXT GROUP. ITEXT GROUP DISCLAIMS THE WARRANTY OF NON INFRINGEMENT
OF THIRD PARTY RIGHTS

This program is distributed in the hope that it will be useful, but
WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY
or FITNESS FOR A PARTICULAR PURPOSE.
See the GNU Affero General Public License for more details.
You should have received a copy of the GNU Affero General Public License
along with this program; if not, see http://www.gnu.org/licenses or write to
the Free Software Foundation, Inc., 51 Franklin Street, Fifth Floor,
Boston, MA, 02110-1301 USA, or download the license from the following URL:
http://itextpdf.com/terms-of-use/

The interactive user interfaces in modified source and object code versions
of this program must display Appropriate Legal Notices, as required under
Section 5 of the GNU Affero General Public License.

In accordance with Section 7(b) of the GNU Affero General Public License,
a covered work must retain the producer line in every PDF that is created
or manipulated using iText.

You can be released from the requirements of the license by purchasing
a commercial license. Buying such a license is mandatory as soon as you
develop commercial activities involving the iText software without
disclosing the source code of your own applications.
These activities include: offering paid services to customers as an ASP,
serving PDFs on the fly in a web application, shipping iText with a closed
source product.

For more information, please contact iText Software Corp. at this
address: sales@itextpdf.com
*/
using System;
using iText.IO.Util;
using iText.Kernel.Geom;
using iText.Kernel.Pdf.Canvas;
using iText.StyledXmlParser.Css.Util;
using iText.Svg.Exceptions;
using iText.Svg.Utils;

namespace iText.Svg.Renderers.Path.Impl {
    /// <summary>Implements lineTo(L) attribute of SVG's path element</summary>
    public class LineTo : AbstractPathShape {
        protected internal String[][] coordinates;

        public LineTo()
            : this(false) {
        }

        public LineTo(bool relative) {
            // (x y)+
            base.relative = relative;
        }

        public override void Draw(PdfCanvas canvas) {
            for (int i = 0; i < coordinates.Length; i++) {
                float x = CssUtils.ParseAbsoluteLength(coordinates[i][0]);
                float y = CssUtils.ParseAbsoluteLength(coordinates[i][1]);
                canvas.LineTo(x, y);
            }
        }

        public override void SetCoordinates(String[] coordinates, Point startPoint) {
            if (coordinates.Length == 0 || coordinates.Length % 2 != 0) {
                throw new ArgumentException(MessageFormatUtil.Format(SvgExceptionMessageConstant.LINE_TO_EXPECTS_FOLLOWING_PARAMETERS_GOT_0
                    , JavaUtil.ArraysToString(coordinates)));
            }
            this.coordinates = new String[coordinates.Length / 2][];
            double[] initialPoint = new double[] { startPoint.GetX(), startPoint.GetY() };
            for (int i = 0; i < coordinates.Length; i += 2) {
                String[] curCoordinates = new String[] { coordinates[i], coordinates[i + 1] };
                if (IsRelative()) {
                    curCoordinates = SvgCoordinateUtils.MakeRelativeOperatorCoordinatesAbsolute(curCoordinates, initialPoint);
                    initialPoint[0] = (float)CssUtils.ParseFloat(curCoordinates[0]);
                    initialPoint[1] = (float)CssUtils.ParseFloat(curCoordinates[1]);
                }
                this.coordinates[i / 2] = curCoordinates;
            }
        }

        public override Point GetEndingPoint() {
            return CreatePoint(coordinates[coordinates.Length - 1][0], coordinates[coordinates.Length - 1][1]);
        }
    }
}
