// app/composables/useAudioPlayer.ts
export const useAudioPlayer = () => {
  const audio = ref<HTMLAudioElement | null>(null);
  const currentUrl = ref<string | null>(null);
  const isPlaying = ref(false);
  const currentTime = ref(0);
  const duration = ref(0);
  const error = ref<string | null>(null);
  const loading = ref(false);
  const volume = ref(0.8);
  const isMuted = ref(false);

  const cleanup = () => {
    if (audio.value) {
      audio.value.pause();
      audio.value.removeEventListener('timeupdate', onTimeUpdate);
      audio.value.removeEventListener('loadedmetadata', onLoadedMetadata);
      audio.value.removeEventListener('play', onPlay);
      audio.value.removeEventListener('pause', onPause);
      audio.value.removeEventListener('waiting', onWaiting);
      audio.value.removeEventListener('canplay', onCanPlay);
      audio.value.removeEventListener('ended', onEnded);
      audio.value.removeEventListener('error', onError);
      audio.value = null;
    }
  };

  const onTimeUpdate = () => {
    if (audio.value) currentTime.value = audio.value.currentTime;
  };

  const onLoadedMetadata = () => {
    if (audio.value) duration.value = audio.value.duration;
  };

  const onPlay = () => {
    isPlaying.value = true;
  };

  const onPause = () => {
    isPlaying.value = false;
  };

  const onWaiting = () => {
    loading.value = true;
  };

  const onCanPlay = () => {
    loading.value = false;
  };

  const onEnded = () => {
    isPlaying.value = false;
    currentTime.value = 0;
  };

  const onError = () => {
    isPlaying.value = false;
    loading.value = false;
    error.value = 'Unable to load or play this audio file.';
  };

  const load = (url: string) => {
    if (currentUrl.value === url && audio.value) return;
    cleanup();
    currentUrl.value = url;
    currentTime.value = 0;
    duration.value = 0;
    isPlaying.value = false;
    error.value = null;
    loading.value = true;

    if (import.meta.client) {
      audio.value = new Audio(url);
      audio.value.volume = volume.value;
      audio.value.muted = isMuted.value;
      audio.value.addEventListener('timeupdate', onTimeUpdate);
      audio.value.addEventListener('loadedmetadata', onLoadedMetadata);
      audio.value.addEventListener('play', onPlay);
      audio.value.addEventListener('pause', onPause);
      audio.value.addEventListener('waiting', onWaiting);
      audio.value.addEventListener('canplay', onCanPlay);
      audio.value.addEventListener('ended', onEnded);
      audio.value.addEventListener('error', onError);
    }
  };

  const play = () => {
    error.value = null;
    // play() returns a promise that rejects if playback is blocked/fails - reflect that in state instead of assuming success.
    audio.value?.play().catch(() => {
      isPlaying.value = false;
      error.value = 'Playback was blocked or failed. Try clicking play again.';
    });
  };

  const pause = () => {
    audio.value?.pause();
  };

  const toggle = () => {
    if (!audio.value) return;
    if (audio.value.paused) play();
    else pause();
  };

  const seek = (time: number) => {
    if (audio.value) audio.value.currentTime = Math.min(Math.max(time, 0), duration.value || time);
  };

  const setVolume = (value: number) => {
    volume.value = Math.min(Math.max(value, 0), 1);
    isMuted.value = volume.value === 0;
    if (audio.value) {
      audio.value.volume = volume.value;
      audio.value.muted = isMuted.value;
    }
  };

  const toggleMute = () => {
    isMuted.value = !isMuted.value;
    if (audio.value) audio.value.muted = isMuted.value;
  };

  onUnmounted(cleanup);

  return {
    isPlaying,
    currentTime,
    duration,
    error,
    loading,
    volume,
    isMuted,
    load,
    play,
    pause,
    toggle,
    seek,
    setVolume,
    toggleMute,
  };
};

