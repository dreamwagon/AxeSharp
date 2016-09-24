using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.dreamwagon.axe
{

    /// <summary>
    /// Any class capable of creating a new AudioInstance.
    /// </summary>
    public interface AudioInstanceFactory
    {
        /// <summary>
        /// Creates a new AudioInstance.
        /// </summary>
        /// <param name="flags">The flags the AudioInstance should have. This 
        /// is used to apply volume, pausing, resumes, pans, and several other 
        /// sound effect changes in one command to many playing effects.</param>
        /// <returns>A new AudioInstance. The AudioInstance must be tracked
        /// by the AudioManager once its played.</returns>
        AudioInstance NewInstance(int flags);
    }

}
