<!-- app/pages/index.vue -->
<template>
  <div class="min-h-screen bg-[#0a0a0a] text-white pt-32 pb-20 px-6 md:px-12 flex flex-col justify-between max-w-7xl mx-auto">
    <div class="mt-12 md:mt-20">

      <!-- Main Display Title -->
      <h1 class="text-7xl md:text-9xl font-serif italic tracking-tight text-white mb-8">
        Suvlet
      </h1>

      <!-- Subtitle -->
      <p class="text-lg md:text-xl text-white/70 max-w-xl font-light leading-relaxed mb-12">
        This is my active music project as music producer / composer.
      </p>

      <!-- Action Links -->
      <div class="grid grid-cols-1 gap-3 max-w-4xl md:flex md:flex-wrap md:items-center md:gap-x-8 md:gap-y-3">
        <NuxtLink 
          to="/releases" 
          class="bg-[#ccff00] text-[#0a0a0a] px-5 py-3.5 font-medium text-sm flex items-center justify-between gap-2 hover:bg-[#b8e600] transition group md:bg-transparent md:text-[#ccff00] md:px-0 md:py-1 md:hover:bg-transparent md:hover:underline md:underline-offset-4"
        >
          Latest Work
          <span class="group-hover:translate-x-1 transition-transform">→</span>
        </NuxtLink>
        
        <NuxtLink 
          to="/profile" 
          class="border border-[#333] hover:border-[#666] text-white px-5 py-3.5 font-medium text-sm flex items-center justify-between gap-2 transition group md:border-0 md:px-0 md:py-1 md:hover:border-0 md:hover:underline md:underline-offset-4"
        >
          About Me
          <span class="group-hover:translate-x-1 transition-transform">→</span>
        </NuxtLink>

        <NuxtLink 
          to="/listen" 
          class="border border-[#333] hover:border-[#666] text-white px-5 py-3.5 font-medium text-sm flex items-center justify-between gap-2 transition group md:border-0 md:px-0 md:py-1 md:hover:border-0 md:hover:underline md:underline-offset-4"
        >
          Genre Previews
          <span class="group-hover:translate-x-1 transition-transform">→</span>
        </NuxtLink>

        <NuxtLink 
          to="/discography" 
          class="border border-[#333] hover:border-[#666] text-white px-5 py-3.5 font-medium text-sm flex items-center justify-between gap-2 transition group md:border-0 md:px-0 md:py-1 md:hover:border-0 md:hover:underline md:underline-offset-4"
        >
          Complete Works
          <span class="group-hover:translate-x-1 transition-transform">→</span>
        </NuxtLink>

        <NuxtLink 
          to="/contact" 
          class="border border-[#333] hover:border-[#666] text-white px-5 py-3.5 font-medium text-sm flex items-center justify-between gap-2 transition group md:border-0 md:px-0 md:py-1 md:hover:border-0 md:hover:underline md:underline-offset-4"
        >
          Contact
          <span class="group-hover:translate-x-1 transition-transform">→</span>
        </NuxtLink>
      </div>
    </div>

    <!-- Decorative Bottom Line Indicator -->
    <div class="mt-20 w-px h-16 bg-[#222] mx-auto hidden md:block"></div>
  </div>
</template>