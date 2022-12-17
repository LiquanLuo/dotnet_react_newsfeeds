import axios, { AxiosResponse } from "axios";
import { Activity } from "../app/model/activity";

const sleep = (delay : number) => {
    return new Promise((resolve) => {
        setTimeout(resolve, delay)
    })
};


axios.defaults.baseURL = 'http://localhost:5000/api';

axios.interceptors.response.use( async response => {
    try {
        await sleep(1000);
        return response;
    } catch (error) {
        console.log(error);
        return await Promise.reject(error);
    }
});

const responseBody = <T>(response: AxiosResponse<T>) => response.data;

const requests = {
    get: <T>(url: string) => axios.get<T>(url).then(responseBody),
    post: <T>(url: string, activity: Activity) => axios.post<T>(url, activity).then(responseBody),
    put: <T>(url: string, activity: Activity) => axios.put<T>(url, activity).then(responseBody),
    delete: <T>(url: string) => axios.delete<T>(url).then(responseBody)
}

const Activities = {
    list: () => requests.get<Activity[]>("/activities"),
    detail: (id : string) => requests.get<Activity>(`/activities/${id}`),
    post: (activity : Activity) => requests.post<void>("/activities", activity),
    put: (activity : Activity) => requests.put<void>(`/activities/${activity.id}`, activity),
    delete: (id : string) => requests.delete<void>(`/activities/${id}`),
    
}

const agent = {
    Activities
}

export default agent;