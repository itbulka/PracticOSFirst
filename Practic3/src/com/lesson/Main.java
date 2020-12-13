package com.lesson;
import java.io.IOException;
import java.util.Random;

public class Main {

    public static void main(String[] args) throws InterruptedException, IOException {

        SimpleDataQueue queue = new SimpleDataQueue();
        Producer producer = new Producer(queue);
        Consumer consumer = new Consumer(queue);

        Thread thread1 = new Thread(producer);
        Thread thread2 = new Thread(producer);
        Thread thread3 = new Thread(producer);
        Thread thread4 = new Thread(consumer);
        Thread thread5 = new Thread(consumer);
        thread1.start();
        thread2.start();
        thread3.start();
        thread4.start();
        thread5.start();
        Thread.sleep(5000);
        producer.shutdown();
        consumer.shutdown();
    }
}

class SimpleDataQueue {

    private volatile int elementsCount;

    public synchronized void add(Integer element) {
        while (elementsCount >= 200) {
            try {
                System.out.println("Waitng");
                wait();
            } catch (InterruptedException e) {
                e.printStackTrace();
            }
        }
        elementsCount++;
        notifyAll();
    }

    public synchronized void remove()
    {
        while (getElementsCount() == 0) {
            try {
                System.out.println("Waiting ");
                wait();
            } catch (InterruptedException e) {
                e.printStackTrace();
            }
        }
        elementsCount--;

        if (elementsCount <= 80) {
            notifyAll();
        }
    }

    public synchronized int getElementsCount() {
        return elementsCount;
    }
}

class Producer implements Runnable {
    private SimpleDataQueue queue;
    private volatile boolean ready = false;

    public Producer(SimpleDataQueue queue) {
        this.queue = queue;
    }

    @Override
    public void run() {
        Random rand = new Random();
        while (!ready) {
            try {
                Thread.sleep(rand.nextInt(100));
            } catch (InterruptedException e) {
                e.printStackTrace();
            }
            queue.add(rand.nextInt(100));
            System.out.println("Thread № "+Thread.currentThread().getName()+" elements: "
                    + queue.getElementsCount());
        }
        System.out.println(" End " + Thread.currentThread().getName());
    }

    public void shutdown() {
        ready = true;

    }
}

class Consumer implements Runnable {

    private volatile SimpleDataQueue queue;
    private volatile boolean ready = false;

    public Consumer(SimpleDataQueue queue) {
        this.queue = queue;
    }

    @Override
    public void run() {
        Random rand = new Random();
        int cnt=0;
        do{
            synchronized(queue){
                cnt=queue.getElementsCount();
                if(cnt>0){
                    queue.remove();
                    System.out.println("Thread № " + Thread.currentThread().getName()+" elements: "
                            + queue.getElementsCount());
                }
                cnt=queue.getElementsCount();

            }
            try {
                Thread.sleep(rand.nextInt(1000));
            } catch (InterruptedException e) {
                e.printStackTrace();
            }
        }while(!ready || cnt>0);
        System.out.println("End " + Thread.currentThread().getName());
    }
    public void shutdown() {
        ready = true;
    }
}