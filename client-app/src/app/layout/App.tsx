import React, { useEffect, useState } from "react";

import { Container } from "semantic-ui-react";
import { Activity } from "../model/activity";
import NavBar from "./NavBar";
import ActivitiyDashboard from "../../features/activities/dashboard/ActivityDashboard";
import { v4 as uuid } from "uuid"
import agent from "../../api/agent";
import LoadingComponent from "./LoadingComponent";

function App() {
  const [activities, setActivities] = useState<Activity[]>([]);
  const [seletctedActivity, setSeletctedActivity] = useState<Activity | undefined>(undefined);
  const [editMode, setEditMode] = useState(false);
  const [isLoading, setIsLoading] = useState(true);
  const [isSubmitting, setIsSubmitting] = useState(false);

  useEffect(() => {
    agent.Activities.list().then((response) => {
      let activities: Activity[] = [];
      response.forEach(
        activity => {
          activity.date = activity.date.split('T')[0];
          activities.push(activity);
        }

      );

      setActivities(activities);
      setIsLoading(false);
    });
  }, []);

  function handleSelectActivity(id: string) {
    setSeletctedActivity(activities.find(x => x.id === id));
  }

  function handleCancelSelectActivity() {
    setSeletctedActivity(undefined);
  }

  function handleFormOn(id?: string) {
    id ? handleSelectActivity(id) : handleCancelSelectActivity();
    setEditMode(true);
  }

  function handleFormClose() {
    setEditMode(false);
  }

  function handleEditCreateActivity(activity: Activity) {
    setIsSubmitting(true);
    if (activity.id) {
      agent.Activities.put(activity).then(() => {
        setActivities([...activities.filter(x => x.id !== activity.id), activity])
        setEditMode(false);
        setSeletctedActivity(activity);
        setIsSubmitting(false);
      });
    }
    else {
      activity = { ...activity, id: uuid() };
      agent.Activities.post(activity).then(() => {
        setActivities([...activities, activity]);
        setEditMode(false);
        setSeletctedActivity(activity);
        setIsSubmitting(false);
      });
    }
  }

  function handleDeleteActivity(id: string) {
    setIsSubmitting(true);
    agent.Activities.delete(id).then(() => {
      setActivities([...activities.filter(x => x.id !== id)]);
      setIsSubmitting(false);
    })
  }

  if (isLoading) return <LoadingComponent content="Loading app..." />

  return (
    <>
      <NavBar openForm={handleFormOn} />
      <Container style={{ marginTop: '7em' }}>
        <ActivitiyDashboard activities={activities}
          selectedActivity={seletctedActivity}
          selectActivity={handleSelectActivity}
          cancelSelectActivity={handleCancelSelectActivity}
          editMode={editMode}
          openForm={handleFormOn}
          closeForm={handleFormClose}
          handleEditCreateActivity={handleEditCreateActivity}
          deleteActivity={handleDeleteActivity}
          isSubmitting={isSubmitting}

        />
      </Container>

    </>
  );
}

export default App;
