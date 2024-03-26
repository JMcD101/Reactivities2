import { observer } from "mobx-react-lite";
import Calendar from "react-calendar";
import { Header, Menu } from "semantic-ui-react";
import { useStore } from "../../../app/stores/store";

export default observer(function ActivityFilters() {
    const {activityStore: {predicate, setPrediacate}} = useStore();

    return (        
        <>
            <Menu vertical size='large' style={{ width: '100%', marginTop: 25 }}>
                <Header icon='filter' attached color='teal' content='Filters' />
                <Menu.Item 
                    content='All Activities'
                    active={predicate.has('all')}
                    onClick={() => setPrediacate('all', 'true')}
                />
                <Menu.Item 
                    content="I'm going" 
                    active={predicate.has('isGoing')}
                    onClick={() => setPrediacate('isGoing', 'true')}
                />
                <Menu.Item
                    content="I'm hosting" 
                    active={predicate.has('isHost')}
                    onClick={() => setPrediacate('isHost', 'true')}
                />
            </Menu>
            <Header />
            <Calendar 
                onChange={(date) => setPrediacate('startDate', date as Date)}
                value={predicate.get('startDate') || new Date}
            />
        </>
    )
})