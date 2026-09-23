<!-- app/components/common/Footer.vue -->
<template>
  <footer class="border-t border-[#222] px-6 md:px-12 py-10">
    <div class="max-w-7xl mx-auto flex flex-col md:flex-row items-center justify-between gap-4 text-xs text-[#666]">
      <p class="uppercase tracking-widest">&copy; {{ year }} Suvlet. All rights reserved.</p>
      <div class="flex items-center gap-6">
        <NuxtLink to="/contact" class="hover:text-[#ccff00] transition">Contact</NuxtLink>
        <NuxtLink to="/listen" class="hover:text-[#ccff00] transition">Listen</NuxtLink>
      </div>
    </div>
  </footer>
</template>

<script setup lang="ts">
const year = new Date().getFullYear();
</script>
