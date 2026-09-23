<!-- app/pages/listen.vue -->
<template>
  <div class="min-h-screen bg-[#0a0a0a] text-white pt-32 pb-20 px-6 md:px-12 max-w-7xl mx-auto">
    <p class="text-xs uppercase tracking-widest text-[#888] mb-2">LISTEN</p>
    <h1 class="text-5xl md:text-6xl font-serif italic mb-4">Genre Previews</h1>

    <!-- Loading / Empty States -->
    <div v-if="store.loading" class="mt-8 border border-[#222] bg-[#121212] p-6 animate-pulse">
      <div class="h-6 w-40 bg-[#222] mb-3"></div>
      <div class="h-3 w-2/3 bg-[#222] mb-6"></div>
      <div class="h-16 bg-[#0a0a0a] border border-[#222]"></div>
      <p class="text-[#888] text-sm mt-4">Loading tracks...</p>
    </div>
    <p v-else-if="!store.tracks.length" class="text-[#888] text-sm">No demo tracks available yet.</p>

    <template v-else>
      <!-- Genre Switcher -->
      <div class="flex flex-wrap items-center gap-3 mb-10">
        <button 
          v-for="genre in store.genres" 
          :key="genre"
          @click="selectedGenre = genre"
          :class="[
            'max-w-full px-5 py-2.5 text-xs font-medium border transition break-words',
            selectedGenre === genre 
              ? 'bg-[#ccff00] text-[#0a0a0a] border-[#ccff00]' 
              : 'border-[#222] text-[#888] hover:text-white'
          ]"
        >
          {{ genre }}
        </button>
      </div>

      <!-- Active Track Player -->
      <AudioPlayer v-if="activeTrack" :track="activeTrack" />
    </template>
  </div>
</template>

<script setup lang="ts">
import AudioPlayer from '~/components/listen/AudioPlayer.vue';
import { useDemoTrackStore } from '~/stores/demotracks.store';

const store = useDemoTrackStore();
const selectedGenre = ref('');

onMounted(async () => {
  await store.fetchDemoTracks();
  if (store.genres.length) {
    selectedGenre.value = store.genres[0] as string;
  }
});

const activeTrack = computed(() =>
  store.tracks.find(t => t.genre === selectedGenre.value) ?? store.tracks[0]
);
</script>