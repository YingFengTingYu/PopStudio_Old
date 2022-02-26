namespace PopStudio.Particles
{
    internal class ParticlesEmitter
    {
        public string Name; //<Name>
        public object Image; //<Image>
        public string ImagePath;
        public int? ImageCol;
        public int? ImageRow;
        public int? ImageFrames; //1
        public int? Animated;
        public int ParticleFlags = 0;
        public int? EmitterType; //1
        public string OnDuration; //<OnDuration>
        public ParticlesTrackNode[] SystemDuration; //<SystemDuration>
        public ParticlesTrackNode[] CrossFadeDuration; //<CrossFadeDuration>
        public ParticlesTrackNode[] SpawnRate; //<SpawnRate>
        public ParticlesTrackNode[] SpawnMinActive; //<SpawnMinActive>
        public ParticlesTrackNode[] SpawnMaxActive; //<SpawnMaxActive>
        public ParticlesTrackNode[] SpawnMaxLaunched; //<SpawnMaxLaunched>
        public ParticlesTrackNode[] EmitterRadius; //<EmitterRadius>
        public ParticlesTrackNode[] EmitterOffsetX; //<EmitterOffsetX>
        public ParticlesTrackNode[] EmitterOffsetY; //<EmitterOffsetY>
        public ParticlesTrackNode[] EmitterBoxX; //<EmitterBoxX>
        public ParticlesTrackNode[] EmitterBoxY; //<EmitterBoxY>
        public ParticlesTrackNode[] EmitterPath; //<EmitterPath>
        public ParticlesTrackNode[] EmitterSkewX; //<EmitterSkewX>
        public ParticlesTrackNode[] EmitterSkewY; //<EmitterSkewY>
        public ParticlesTrackNode[] ParticleDuration; //<ParticleDuration>
        public ParticlesTrackNode[] SystemRed; //<SystemRed>
        public ParticlesTrackNode[] SystemGreen; //<SystemGreen>
        public ParticlesTrackNode[] SystemBlue; //<SystemBlue>
        public ParticlesTrackNode[] SystemAlpha; //<SystemAlpha>
        public ParticlesTrackNode[] SystemBrightness; //<SystemBrightness>
        public ParticlesTrackNode[] LaunchSpeed; //<LaunchSpeed>
        public ParticlesTrackNode[] LaunchAngle; //<LaunchAngle>
        public ParticlesField[] Field; //<Field>
        public ParticlesField[] SystemField; //<SystemField>
        public ParticlesTrackNode[] ParticleRed; //<ParticleRed>
        public ParticlesTrackNode[] ParticleGreen; //<ParticleGreen>
        public ParticlesTrackNode[] ParticleBlue; //<ParticleBlue>
        public ParticlesTrackNode[] ParticleAlpha; //<ParticleAlpha>
        public ParticlesTrackNode[] ParticleBrightness; //<ParticleBrightness>
        public ParticlesTrackNode[] ParticleSpinAngle; //<ParticleSpinAngle>
        public ParticlesTrackNode[] ParticleSpinSpeed; //<ParticleSpinSpeed>
        public ParticlesTrackNode[] ParticleScale; //<ParticleScale>
        public ParticlesTrackNode[] ParticleStretch; //<ParticleStretch>
        public ParticlesTrackNode[] CollisionReflect; //<CollisionReflect>
        public ParticlesTrackNode[] CollisionSpin; //<CollisionSpin>
        public ParticlesTrackNode[] ClipTop; //<ClipTop>
        public ParticlesTrackNode[] ClipBottom; //<ClipBottom>
        public ParticlesTrackNode[] ClipLeft; //<ClipLeft>
        public ParticlesTrackNode[] ClipRight; //<ClipRight>
        public ParticlesTrackNode[] AnimationRate; //<AnimationRate>
    }
}
