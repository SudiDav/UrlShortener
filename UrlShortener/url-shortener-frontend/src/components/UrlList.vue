<template>
    <div class="url-list">
      <h2 class="text-2xl font-semibold mb-4">Recent URLs</h2>
      <div v-if="isLoading" class="text-gray-500">Loading...</div>
      <div v-else-if="urls.length === 0" class="text-gray-500">No recent URLs found.</div>
      <ul v-else class="space-y-4 mb-4">
        <li v-for="url in urls" :key="url.id" class="bg-white shadow-md rounded-lg p-4">
          <div class="flex justify-between items-center">
            <a :href="getShortUrl(url.shortUrlId)" target="_blank" class="text-blue-500 hover:underline font-semibold">
              {{ getShortUrlDisplay(url.shortUrlId) }}
            </a>
            <span class="text-gray-500">{{ url.accessCount }} clicks</span>
          </div>
          <div class="text-gray-600 text-sm mt-2 truncate">
            Original: {{ url.originalUrl }}
          </div>
          <div class="text-gray-400 text-xs mt-1">
            Expires: {{ new Date(url.expiryDate).toLocaleString() }}
          </div>
        </li>
      </ul>
      <div class="flex justify-between items-center mt-4">
        <button 
          @click="prevPage" 
          :disabled="currentPage === 1" 
          class="px-4 py-2 bg-blue-500 text-white rounded disabled:opacity-50"
        >
          Previous
        </button>
        <span>Page {{ currentPage }} of {{ totalPages }}</span>
        <button 
          @click="nextPage" 
          :disabled="currentPage === totalPages" 
          class="px-4 py-2 bg-blue-500 text-white rounded disabled:opacity-50"
        >
          Next
        </button>
      </div>
    </div>
  </template>

<script setup lang="ts">
import { onMounted, ref, computed } from 'vue';
import { getRecentUrls, ShortenedUrl } from '../services/api';
import { DEFAULT_PAGE, DEFAULT_PAGE_SIZE } from '../constants';

const urls = ref<ShortenedUrl[]>([]);
const isLoading = ref(true);
const currentPage = ref(DEFAULT_PAGE);
const pageSize = ref(DEFAULT_PAGE_SIZE);
const totalCount = ref(0);

const totalPages = computed(() => Math.ceil(totalCount.value / pageSize.value));

const fetchUrls = async () => {
  isLoading.value = true;
  try {
    const response = await getRecentUrls(currentPage.value, pageSize.value);
    urls.value = response.urls;
    totalCount.value = response.totalCount;
  } catch (error) {
    console.error('Error fetching recent URLs:', error);
  } finally {
    isLoading.value = false;
  }
};

onMounted(fetchUrls);

const prevPage = () => {
  if (currentPage.value > 1) {
    currentPage.value--;
    fetchUrls();
  }
};

const nextPage = () => {
  if (currentPage.value < totalPages.value) {
    currentPage.value++;
    fetchUrls();
  }
};

const getShortUrl = (shortUrlId: string) => `${import.meta.env.VITE_API_BASE_URL}/urls/${shortUrlId}`;
const getShortUrlDisplay = (shortUrlId: string) => `${new URL(import.meta.env.VITE_API_BASE_URL).host}/urls/${shortUrlId}`;
</script>