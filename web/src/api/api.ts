import axios, { AxiosRequestConfig } from 'axios';
export const axiosInstance = axios.create({
  baseURL: process.env.REACT_APP_API_URL,
});

export const get = <Response = any>(url: string, config: AxiosRequestConfig<void> = {}) =>
  axiosInstance.get<Response>(url, config);

export const post = <Response = any, Body = any>(url: string, body: Body, config: AxiosRequestConfig<Body> = {}) =>
  axiosInstance.post<Response>(url, body, config);

export const put = <Response = any, Body = any>(url: string, body: Body, config: AxiosRequestConfig<Body> = {}) =>
  axiosInstance.put<Response>(url, body, config);

export const deleteRequest = <Response = any, Body = any>(url: string, config: AxiosRequestConfig<Body> = {}) =>
  axiosInstance.delete<Response>(url, config);
