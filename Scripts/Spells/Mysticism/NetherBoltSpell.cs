using System;
using Server.Targeting;

namespace Server.Spells.Mysticism
{
	public class NetherBoltSpell : MysticSpell
	{
		private static SpellInfo m_Info = new SpellInfo(
			"Nether Bolt", "In Corp Ylem",
			-1,
			9002,
			Reagent.BlackPearl,
			Reagent.SulfurousAsh
		);

		public override TimeSpan CastDelayBase { get { return TimeSpan.FromSeconds( 0.75 ); } }

		public override double RequiredSkill { get { return 0.0; } }
		public override int RequiredMana { get { return 4; } }

		public override bool DelayedDamage { get{ return true; } }

		public NetherBoltSpell( Mobile caster, Item scroll )
			: base( caster, scroll, m_Info )
		{
		}

		public override void OnCast()
		{
			Caster.Target = new InternalTarget( this );
		}

		public void Target( Mobile m )
		{
			if ( CheckHSequence( m ) )
			{
				/* Fires a bolt of nether energy at the Target,
				 * dealing chaos damage.
				 */

				SpellHelper.Turn( Caster, m );

				SpellHelper.CheckReflect( 0, Caster, ref m );

				Caster.MovingParticles( m, 0x36D4, 7, 0, false, true, 0x49A, 0, 0x251E, 0x4A7A, 0x160, 0 );
				Caster.PlaySound( 0x211 );

				SpellHelper.ChaosDamage( this, m, GetNewAosDamage( 10, 1, 4, m ) );
			}

			FinishSequence();
		}

		private class InternalTarget : Target
		{
			private NetherBoltSpell m_Owner;

			public InternalTarget( NetherBoltSpell owner )
				: base( 12, false, TargetFlags.Harmful )
			{
				m_Owner = owner;
			}

			protected override void OnTarget( Mobile from, object o )
			{
				if ( o is Mobile )
					m_Owner.Target( (Mobile) o );
			}

			protected override void OnTargetFinish( Mobile from )
			{
				m_Owner.FinishSequence();
			}
		}
	}
}
