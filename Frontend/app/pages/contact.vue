<!-- app/pages/contact.vue -->
<template>
  <div class="min-h-screen bg-[#0a0a0a] text-white pt-32 pb-20 px-6">
    <div class="max-w-4xl mx-auto">
      <p class="text-xs uppercase tracking-widest text-[#888] mb-2">CONTACT</p>
      <h1 class="text-5xl md:text-6xl font-serif italic mb-6">Get in Touch</h1>

      <p class="text-white/70 max-w-xl text-lg mb-12 leading-relaxed">
        For bookings, collaborations, sync licensing, or anything else — reach out directly.
      </p>

      <!-- Minimal Information List -->
      <div class="space-y-6 max-w-2xl">
        <div v-if="email" class="flex items-center justify-between py-5 border-b border-[#222]">
          <span class="text-xs uppercase tracking-wider text-[#888]">EMAIL</span>
          <a :href="`mailto:${email}`" class="text-base font-medium hover:text-[#ccff00] transition">{{ email }}</a>
        </div>

        <div v-if="profile?.instagramUrl" class="flex items-center justify-between py-5 border-b border-[#222]">
          <span class="text-xs uppercase tracking-wider text-[#888]">INSTAGRAM</span>
          <a :href="profile.instagramUrl" target="_blank" class="text-base font-medium hover:text-[#ccff00] transition">Instagram</a>
        </div>

        <div v-if="profile?.spotifyUrl" class="flex items-center justify-between py-5 border-b border-[#222]">
          <span class="text-xs uppercase tracking-wider text-[#888]">SPOTIFY</span>
          <a :href="profile.spotifyUrl" target="_blank" class="text-base font-medium hover:text-[#ccff00] transition">Spotify</a>
        </div>

        <div v-if="profile?.soundcloudUrl" class="flex items-center justify-between py-5 border-b border-[#222]">
          <span class="text-xs uppercase tracking-wider text-[#888]">SOUNDCLOUD</span>
          <a :href="profile.soundcloudUrl" target="_blank" class="text-base font-medium hover:text-[#ccff00] transition">SoundCloud</a>
        </div>

        <div v-if="profile?.youtubeUrl" class="flex items-center justify-between py-5 border-b border-[#222]">
          <span class="text-xs uppercase tracking-wider text-[#888]">YOUTUBE</span>
          <a :href="profile.youtubeUrl" target="_blank" class="text-base font-medium hover:text-[#ccff00] transition">YouTube</a>
        </div>
      </div>

      <!-- Availability Box -->
      <div v-if="status" class="mt-12 p-6 border-l-2 border-[#ccff00] bg-[#121212] max-w-2xl">
        <p class="text-xs uppercase tracking-widest text-[#888] mb-1">CURRENT STATUS</p>
        <p class="text-white font-medium">{{ status }}</p>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { useProfileStore } from '~/stores/profile.store';
import { fetchApi } from '~/services/api';

const store = useProfileStore();
const status = ref('');

onMounted(async () => {
  store.fetchProfile();
  try {
    const setting = await fetchApi<{ key: string; value: string }>('/api/settings/AvailabilityStatus');
    status.value = setting.value;
  } catch {
    // no availability status has been set yet
  }
});

const profile = computed(() => store.profile);
const email = computed(() => store.profile?.email || '');
</script>