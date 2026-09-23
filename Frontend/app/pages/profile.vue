<!-- app/pages/profile.vue -->
<template>
  <div class="min-h-screen bg-[#0a0a0a] text-white pt-32 pb-20 px-6 md:px-12 max-w-7xl mx-auto">
    <div class="grid grid-cols-1 lg:grid-cols-12 gap-12 items-start">
      
      <!-- Profile Image Block -->
      <div class="lg:col-span-5 aspect-[4/5] bg-[#121212] border border-[#222] relative overflow-hidden group">
        <!-- ใส่ URL รูปภาพโปรไฟล์จริงที่นี่ -->
        <img 
          src="https://pub-9a3747800be64c58884beb75cd0f8542.r2.dev/1.png" 
          alt="Suvlet" 
          class="w-full h-full object-cover group-hover:scale-105 transition-transform"
        />
      </div>

      <!-- Profile Details -->
      <div class="lg:col-span-7 flex flex-col justify-between">
        <div>
          <h1 class="text-6xl md:text-7xl font-serif italic mb-2">Suvlet</h1>
          <p class="text-sm text-[#888] tracking-widest uppercase mb-8">Bangkok, Thailand</p>

          <div class="space-y-6 text-white/70 text-base md:text-lg leading-relaxed font-light mb-10">
            <p v-if="store.loading" class="text-[#888]">Loading profile...</p>
            <p v-else class="whitespace-pre-line">{{ store.profile?.description || 'Suvlet is an independent electronic and ambient music producer.' }}</p>
            <p v-if="store.profile?.email" class="text-white font-normal">
              reach out at
              <a :href="`mailto:${store.profile.email}`" class="hover:text-[#ccff00] transition">{{ store.profile.email }}</a>
            </p>
          </div>

        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { useProfileStore } from '~/stores/profile.store';

const store = useProfileStore();

onMounted(() => {
  store.fetchProfile();
});
</script>