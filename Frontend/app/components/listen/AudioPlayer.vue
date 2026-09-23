<!-- app/components/listen/AudioPlayer.vue -->
<template>
  <div class="p-6 md:p-8 bg-[#121212] border border-[#222] rounded-none flex flex-col gap-6">
    <div class="flex items-start justify-between">
      <div>
        <h3 class="text-2xl font-serif italic text-white">{{ track.name }}</h3>
        <p class="text-xs text-[#888] mt-1">{{ track.genre }} • {{ track.description }}</p>
      </div>

      <button 
        @click="player.toggle()"
        :disabled="player.loading.value"
        class="w-12 h-12 bg-[#ccff00] text-[#0a0a0a] flex items-center justify-center hover:opacity-90 transition font-bold"
      >
        <span v-if="player.loading.value" class="text-xs">...</span>
        <span v-else-if="!player.isPlaying.value">▶</span>
        <span v-else>❚❚</span>
      </button>
    </div>

    <!-- Waveform Bars (click/drag to seek) -->
    <div
      ref="waveformEl"
      class="w-full h-16 flex items-end gap-1 px-2 py-1 bg-[#0a0a0a] border border-[#222] cursor-pointer select-none"
      @click="handleSeek"
      @mousedown="isDragging = true"
      @mousemove="handleDrag"
      @mouseup="isDragging = false"
      @mouseleave="isDragging = false"
    >
      <div 
        v-for="(bar, index) in waveform" 
        :key="index" 
        class="flex-1 transition-all duration-150 pointer-events-none"
        :class="progressRatio > index / waveform.length ? 'bg-[#ccff00]' : 'bg-[#444]'"
        :style="{ height: `${bar}%` }"
      ></div>
    </div>

    <div class="flex items-center justify-between text-[10px] text-[#666] -mt-4">
      <span>{{ formatTime(player.currentTime.value) }}</span>
      <span>{{ formatTime(player.duration.value) }}</span>
    </div>

    <div class="flex items-center gap-3 -mt-2">
      <button
        type="button"
        @click="player.toggleMute()"
        class="w-8 h-8 border border-[#333] text-xs text-white hover:border-[#ccff00] hover:text-[#ccff00] transition"
        :aria-label="player.isMuted.value ? 'Unmute' : 'Mute'"
      >
        {{ player.isMuted.value ? 'M' : 'V' }}
      </button>
      <input
        :value="player.volume.value"
        type="range"
        min="0"
        max="1"
        step="0.01"
        aria-label="Volume"
        class="w-full accent-[#ccff00] cursor-pointer"
        @input="player.setVolume(Number(($event.target as HTMLInputElement).value))"
      />
      <span class="w-9 text-right text-[10px] text-[#888]">{{ Math.round(player.volume.value * 100) }}%</span>
    </div>

    <p v-if="player.error.value" class="text-xs text-red-400">{{ player.error.value }}</p>
    <p v-else-if="player.loading.value" class="text-xs text-[#888]">Loading audio...</p>
  </div>
</template>

<script setup lang="ts">
import type { DemoTrack } from '~/types';

const props = defineProps<{ track: DemoTrack }>();
const player = useAudioPlayer();

const waveformEl = ref<HTMLElement | null>(null);
const isDragging = ref(false);

const waveform = computed(() => {
  if (props.track.waveformDataJson) {
    try {
      const parsed = JSON.parse(props.track.waveformDataJson);
      if (Array.isArray(parsed) && parsed.length) return parsed as number[];
    } catch {
      // fall through to generated waveform
    }
  }
  // Deterministic pseudo-random bars seeded by track id so they don't reshuffle on re-render.
  let seed = 0;
  for (const char of props.track.id) seed = (seed * 31 + char.charCodeAt(0)) >>> 0;
  return Array.from({ length: 48 }, () => {
    seed = (seed * 1103515245 + 12345) >>> 0;
    return 20 + (seed % 80);
  });
});

const progressRatio = computed(() => {
  if (!player.duration.value) return 0;
  return player.currentTime.value / player.duration.value;
});

const formatTime = (seconds: number) => {
  if (!seconds || !Number.isFinite(seconds)) return '0:00';
  const mins = Math.floor(seconds / 60);
  const secs = Math.floor(seconds % 60);
  return `${mins}:${secs.toString().padStart(2, '0')}`;
};

const seekFromEvent = (event: MouseEvent) => {
  if (!waveformEl.value || !player.duration.value) return;
  const rect = waveformEl.value.getBoundingClientRect();
  const ratio = Math.min(Math.max((event.clientX - rect.left) / rect.width, 0), 1);
  player.seek(ratio * player.duration.value);
};

const handleSeek = (event: MouseEvent) => seekFromEvent(event);
const handleDrag = (event: MouseEvent) => {
  if (isDragging.value) seekFromEvent(event);
};

watch(
  () => props.track.audioUrl,
  (url) => {
    if (url) player.load(url);
  },
  { immediate: true }
);
</script>