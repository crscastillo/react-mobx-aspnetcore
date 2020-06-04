import { RootStore } from "./rootStore";
import { observable, action, reaction } from "mobx";

export default class CommonStore {
    rootStore: RootStore

    constructor(rootStore: RootStore) {
        this.rootStore = rootStore;

        // a reaction runs whenever the observable is set
        reaction(
            () => this.token,
            token => {
                if(token) {
                    window.localStorage.setItem('reactivities_jwt', token); // any  key we want
                }
                else {
                    window.localStorage.removeItem('reactivities_jwt');
                }
            }
            )
    }

    @observable token: string | null = window.localStorage.getItem('reactivities_jwt');
    @observable appLoaded = false;

    @action setToken = (token: string | null) => {        
        this.token = token;
    }

    @action setAppLoaded = () => {
        this.appLoaded = true;
    }
}