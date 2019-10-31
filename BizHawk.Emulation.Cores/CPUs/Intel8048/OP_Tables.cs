using System;
using BizHawk.Common.NumberExtensions;

namespace BizHawk.Emulation.Common.Components.I8048
{
	public partial class I8048
	{
		// this contains the vectors of instrcution operations
		// NOTE: This list is NOT confirmed accurate for each individual cycle
		public void ILLEGAL()
		{
			PopulateCURINSTR(IDLE,
							IDLE,
							IDLE,
							IDLE);

			IRQS = 4;
		}

		public void OP_IMP(ushort oper)
		{
			PopulateCURINSTR(IDLE,
							IDLE,
							IDLE,
							oper);

			IRQS = 4;
		}

		public void OP_R_IMP(ushort oper, ushort reg)
		{
			PopulateCURINSTR(IDLE,
							IDLE,
							IDLE,
							oper, reg);

			IRQS = 4;
		}


		public void OP_A_R(ushort oper, ushort reg)
		{
			PopulateCURINSTR(IDLE,
							IDLE,
							IDLE,
							oper,A,reg);

			IRQS = 4;
		}

		public void OP_A_DIR(ushort oper)
		{
			PopulateCURINSTR(IDLE,
							IDLE,
							IDLE,
							RD, ALU, PC,
							INC16, PC,
							IDLE,
							IDLE,
							IDLE,
							oper, A, ALU);

			IRQS = 9;
		}

		public void OP_PB_DIR(ushort oper, ushort reg)
		{
			PopulateCURINSTR(IDLE,
							IDLE,
							IDLE,
							RD, ALU, PC,
							INC16, PC,
							IDLE,
							IDLE,
							IDLE,
							oper, reg, ALU);

			IRQS = 9;
		}

		public void OP_EXP_A(ushort oper, ushort reg)
		{
			// Lower 4 bits only		
			PopulateCURINSTR(IDLE,
							IDLE,
							IDLE,
							TR, ALU, A,
							IDLE,
							IDLE,
							MSK, ALU,
							IDLE,
							oper, reg, ALU);

			IRQS = 9;
		}

		public void CALL(ushort dest_h)
		{
			// Lower 4 bits only		
			PopulateCURINSTR(IDLE,
							IDLE,
							IDLE,
							TR, ALU, A,
							IDLE,
							IDLE,
							MSK, ALU,
							IDLE,
							ALU);

			IRQS = 9;
		}
	}
}
