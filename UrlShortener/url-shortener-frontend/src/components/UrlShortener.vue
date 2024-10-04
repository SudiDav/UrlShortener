<template>
  <div class="url-shortener">
    <div class="flex flex-col gap-4">
      <input 
        v-model="originalUrl" 
        placeholder="Enter a long URL" 
        class="px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-blue-500 transition duration-150 ease-in-out"
      />
      <input 
        v-model="customShortId" 
        placeholder="Custom short ID (optional)" 
        class="px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-blue-500 transition duration-150 ease-in-out"
      />
      <input 
        v-model="ttl" 
        type="number" 
        placeholder="Time To Live in minutes (optional)" 
        class="px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-blue-500 transition duration-150 ease-in-out"
      />
      <button 
        @click="shortenUrlHandler" 
        :disabled="!isValidUrl || isLoading" 
        class="px-6 py-2 bg-blue-600 text-white font-semibold rounded-lg shadow-md hover:bg-blue-700 focus:outline-none focus:ring-2 focus:ring-blue-500 focus:ring-opacity-75 transition duration-150 ease-in-out disabled:opacity-50 disabled:cursor-not-allowed"
      >
        <span v-if="isLoading" class="flex items-center justify-center">
          <svg class="animate-spin -ml-1 mr-3 h-5 w-5 text-white" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24">
            <circle class="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" stroke-width="4"></circle>
            <path class="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"></path>
          </svg>
          Shortening...
        </span>
        <span v-else>Shorten</span>
      </button>
    </div>
    <div v-if="shortUrlId" class="mt-4 p-4 bg-green-100 border border-green-200 rounded-lg">
      <p class="text-green-800 font-semibold">Shortened URL:</p>
      <a :href="getShortUrl(shortUrlId)" target="_blank" class="text-blue-600 hover:underline break-all">
        {{ getShortUrlDisplay(shortUrlId) }}
      </a>
    </div>
    <div v-if="error" class="mt-4 p-4 bg-red-100 border border-red-200 rounded-lg text-red-700">
      {{ error }}
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, computed } from 'vue'
import { shortenUrl } from '../services/api'

const originalUrl = ref('')
const customShortId = ref('')
const ttl = ref<number | null>(null)
const shortUrlId = ref('')
const error = ref('')
const isLoading = ref(false)

const isValidUrl = computed(() => {
  try {
    new URL(originalUrl.value)
    return true
  } catch {
    return false
  }
})

const shortenUrlHandler = async () => {
  error.value = ''
  if (!isValidUrl.value) {
    error.value = 'Please enter a valid URL'
    return
  }
  isLoading.value = true
  try {
    const result = await shortenUrl({
      originalUrl: originalUrl.value,
      customShortId: customShortId.value || undefined,
      ttl: ttl.value || undefined
    })
    shortUrlId.value = result.shortUrlId
  } catch (err: any) {
    error.value = 'Error shortening URL. Please try again.'
    console.error('Error shortening URL:', err)
    if (err.response) {
      console.error('Response data:', err.response.data)
      console.error('Response status:', err.response.status)
    } else if (err.request) {
      console.error('No response received:', err.request)
    } else {
      console.error('Error message:', err.message)
    }
  } finally {
    isLoading.value = false
  }
}

const getShortUrl = (id: string) => `${import.meta.env.VITE_API_BASE_URL}/urls/${id}`
const getShortUrlDisplay = (id: string) => `${new URL(import.meta.env.VITE_API_BASE_URL).host}/urls/${id}`
</script>